using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;


public static class FhirParticipantAdapter
{
	public static Participant FromJson (string json)
	{ 

		var parser = new FhirJsonParser();
		var fhirPatient = parser.Parse<Hl7.Fhir.Model.Patient>(json);

		Participant participant = new Participant{
			Id = Guid.Parse(fhirPatient.Id),
			FirstName = "",
			LastName = ""
		};

		foreach (var fhirId in fhirPatient.Identifier) {
			if (fhirId.System == "https://mibplatform.nl/fhir/mib/identifier") {
				participant.Id =  Guid.Parse(fhirId.Value);
			}
		}

		foreach (var fhirName in fhirPatient.Name) {
			foreach (var fhirGivenName in fhirName.Given) {
				participant.FirstName = fhirGivenName;
			}
			participant.LastName = fhirName.Family;
		}

		//participant.Birth = DateOnly.Parse(fhirPatient.BirthDate);

		participant.MaritalStatus = "M";

		participant.IsActive = fhirPatient.Active ?? false;

		return participant;
	}

	public static string ToJson (Participant participant)
	{ 
 		// create questionnaire fhir object
		var fhirPatient = new Hl7.Fhir.Model.Patient
		{
			Id = participant.Id.ToString()
		};


		var fhirPatientName = new Hl7.Fhir.Model.HumanName();
		fhirPatientName.Given.Append(participant.FirstName);
		fhirPatientName.Family = participant.LastName;
		fhirPatient.Name.Add(fhirPatientName);

		//fhirPatient.BirthDate = participant.Birth.ToString();
		// todo parse entity string to enum
		fhirPatient.Gender = Hl7.Fhir.Model.AdministrativeGender.Unknown;
		
		/*
		* Weight needs to be an observation https://www.hl7.org/fhir/observation-example.json.html
		* Height needs to be an observation 
		*/

		fhirPatient.MaritalStatus = new Hl7.Fhir.Model.CodeableConcept();
		fhirPatient.MaritalStatus.Coding.Add(new Hl7.Fhir.Model.Coding{
				System = "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus",
				Code = "M",
				Display = "Maried"

		});

		fhirPatient.Active = participant.IsActive;

		// serialize and return fhir object
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirPatient);
	}

}