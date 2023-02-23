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
			// created at
			Issued = new DateTimeOffset(execution.CreatedAt.Year, execution.CreatedAt.Month, execution.CreatedAt.Day, 0, 0, 0, TimeSpan.FromHours(0)),
			// is done
			Status = execution.IsDone?Hl7.Fhir.Model.ObservationStatus.Final:Hl7.Fhir.Model.ObservationStatus.Preliminary,
			// reason annotation
			Note = new List<Hl7.Fhir.Model.Annotation> {
				new Hl7.Fhir.Model.Annotation {
					Text = new Hl7.Fhir.Model.Markdown(execution.Reason)
				}
			},
			// amount component
			Component = new List<Hl7.Fhir.Model.Observation.ComponentComponent> {	
				new Hl7.Fhir.Model.Observation.ComponentComponent{
					Code = new Hl7.Fhir.Model.CodeableConcept {
						Coding = new List<Hl7.Fhir.Model.Coding> {
							new Hl7.Fhir.Model.Coding {
								System = "http://loinc.org",
								Code = "90897-0",
								Display = "Percent of task completed"
							}
						}
					},
					Value = new Hl7.Fhir.Model.Integer(execution.Amount)
				}
			}	
		};

		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirObservation);
	}

	private static Execution FhirObservationToExecution(Hl7.Fhir.Model.Observation fhirObservation) {

		// Get amount
		int amount = 0;
		foreach (var fhirObservationComponent in fhirObservation.Component) {
			foreach (var fhirCoding in fhirObservationComponent.Code.Coding) {
				if (fhirCoding.Code == "90897-0") {
					amount =  Int32.Parse(fhirObservationComponent.Value.ToString() ?? "0");
				}
			}
		}

		// get reason
		var reason = "";
		foreach (var annotation in fhirObservation.Note) {
			reason = annotation.Text.ToString();
		}

		// build execution object
		var execution = new Execution {
			Id = Guid.Parse(fhirObservation.Id),
			CreatedAt = new DateOnly(fhirObservation.Issued.Value.Year, fhirObservation.Issued.Value.Month, fhirObservation.Issued.Value.Day),
			IsDone = fhirObservation.Status == Hl7.Fhir.Model.ObservationStatus.Final,
			Amount = amount,
			Reason = reason,
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
