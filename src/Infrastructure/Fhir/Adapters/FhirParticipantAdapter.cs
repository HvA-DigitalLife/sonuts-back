using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;


public static class FhirParticipantAdapter
{
	public static Participant FromJson (string json)
	{ 
		// create questionnaire instance
		Participant participant = new Participant();
         
        
		var parser = new FhirJsonParser();
		var fhirParticipant = parser.Parse<Hl7.Fhir.Model.Patient>(json);

		foreach (var fhirId in fhirParticipant.Identifier) {
			if (fhirId.System == "https://mibplatform.nl/fhir/mib/identifier") {
				participant.Id =  Guid.Parse(fhirId.Value);
			}
		}

		foreach (var fhirName in fhirParticipant.Name) {
			foreach (var fhirGivenName in fhirName.Given) {
				participant.FirstName = fhirGivenName;
			}
			participant.LastName = fhirName.Family;
		}

		participant.Birth = DateOnly.Parse(fhirParticipant.BirthDate);

		participant.MaritalStatus = "M";

		participant.IsActive = fhirParticipant.Active ?? false;

		return participant;
	}

	public static string ToJson (Participant participant)
	{ 
 		// create questionnaire fhir object
		var fhirParticipant = new Hl7.Fhir.Model.Patient
		{
	
		};

				// add identifier
		fhirParticipant.Identifier.Add(new Hl7.Fhir.Model.Identifier {
				System = "https://mibplatform.nl/fhir/mib/identifier",
				Value = participant.Id.ToString()
			});


		var fhirParticipantName = new Hl7.Fhir.Model.HumanName();
		fhirParticipantName.Given.Append(participant.FirstName);
		fhirParticipantName.Family = participant.LastName;
		fhirParticipant.Name.Add(fhirParticipantName);

		fhirParticipant.BirthDate = participant.Birth.ToString();
		// todo parse entity string to enum
		fhirParticipant.Gender = Hl7.Fhir.Model.AdministrativeGender.Unknown;
		
		/*
		* Weight needs to be an observation https://www.hl7.org/fhir/observation-example.json.html
		* Height needs to be an observation 
		*/

		fhirParticipant.MaritalStatus = new Hl7.Fhir.Model.CodeableConcept();
		fhirParticipant.MaritalStatus.Coding.Add(new Hl7.Fhir.Model.Coding{
				System = "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus",
				Code = "M",
				Display = "Maried"

		});

		fhirParticipant.Active = participant.IsActive;

		// serialize and return fhir object
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirParticipant);
	}

}