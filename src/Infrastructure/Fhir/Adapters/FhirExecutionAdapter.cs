using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirExecutionAdapter
{

	public static List<Execution> FromJsonBundle(string json)
	{
		// create list of recommendations
		var executionList = new List<Execution>();

		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.Observation)) {
				var executionEntry = (Hl7.Fhir.Model.Observation) fhirBundleEntry.Resource;
				// Convert Fhir plan definition object and add to list
				executionList.Add(FhirObservationToExecution(executionEntry));
			}
		}

		// return list of recommendations
		return executionList;
	}


	public static Execution FromJson (string json)
	{       
		var fhirJsonParser = new FhirJsonParser();
		// parse plan definition resource and return Recommendation object
		return FhirObservationToExecution(fhirJsonParser.Parse<Hl7.Fhir.Model.Observation>(json));
	}

	public static string ToJson ( Execution execution )
	{
		// create plan definion and meta data
		var fhirObservation= new Hl7.Fhir.Model.Observation{
			Id = execution.Id.ToString(),
			Issued = execution.CreatedAt,
			Status = execution.IsDone?Hl7.Fhir.Model.ObservationStatus.Final:Hl7.Fhir.Model.ObservationStatus.Preliminary
		};

		
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirObservation);
	}

	private static Execution FhirObservationToExecution(Hl7.Fhir.Model.Observation fhirObservation) {

		var execution = new Execution {
			Id = Guid.Parse(fhirObservation.Id),
			CreatedAt = DateTime.Parse(fhirObservation.IssuedElement.ToString()),
			IsDone = fhirObservation.Status == Hl7.Fhir.Model.ObservationStatus.Final,
			Amount = 0, // to-do: implement
			Goal = new Goal{ // to-do: add identifier from reference
				Activity = new Activity{
					Name = "",
					Image = new Image {
						Extension = "NA"
					}
				},
				FrequencyAmount = 0,
				Moment = new Moment {
					Day = DayOfWeek.Monday,
					Type = MomentType.During
				}
			}
		};
	
		return execution;
	}

}
