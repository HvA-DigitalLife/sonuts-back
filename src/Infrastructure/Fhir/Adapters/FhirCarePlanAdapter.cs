using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirCarePlanAdapter
{

	public static List<CarePlan> FromJsonBundle(string json)
	{
		// create list of recommendations
		var carePlanList = new List<CarePlan>();

		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.CarePlan)) {
				var carePlanEntry = (Hl7.Fhir.Model.CarePlan) fhirBundleEntry.Resource;
				// Convert Fhir plan definition object and add to list
				carePlanList.Add(FhirCarePlanToCarePlan(carePlanEntry));
			}
		}

		// return list of recommendations
		return carePlanList;
	}




	public static CarePlan FromJson (string json)
	{       
		var fhirJsonParser = new FhirJsonParser();
		// parse plan definition resource and return Recommendation object
		return FhirCarePlanToCarePlan(fhirJsonParser.Parse<Hl7.Fhir.Model.CarePlan>(json));
	}

	public static string ToJson ( CarePlan carePlan )
	{
		// create plan definion and meta data
		var fhirCarePlan= new Hl7.Fhir.Model.CarePlan{
			Id = carePlan.Id.ToString()
		};

		fhirCarePlan.Period =  new Hl7.Fhir.Model.Period {
			Start = carePlan.Start.ToString("yyyy-MM-dd"),
			End = carePlan.End.ToString("yyyy-MM-dd")
		};


		foreach (var goal in carePlan.Goals) {
			var fhirActivity = new Hl7.Fhir.Model.CarePlan.ActivityComponent();
			fhirActivity.Detail = new Hl7.Fhir.Model.CarePlan.DetailComponent();
			fhirActivity.Detail.Goal.Add(new Hl7.Fhir.Model.ResourceReference{Reference = "Goal/" + goal.Id});
			fhirCarePlan.Activity.Add(fhirActivity);
		}

          
		// serialize and return
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirCarePlan);
	}

	private static CarePlan FhirCarePlanToCarePlan(Hl7.Fhir.Model.CarePlan fhirCarePlan) {
		// create interventionPlan model and add meta data
		var carePlan = new CarePlan{
			Id = Guid.Parse(fhirCarePlan.Id),
			Participant = new Participant{
				FirstName = "",
				LastName = ""
			}
		};

		carePlan.Start = DateOnly.Parse(fhirCarePlan.Period.Start);
		carePlan.End = DateOnly.Parse(fhirCarePlan.Period.End);
        
		foreach (var fhirActivity in fhirCarePlan.Activity) {
		    foreach (var fhirGoal in fhirActivity.Detail.Goal) {
				// to-do, see if we can do something different with this.
				var goal = new Goal{
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
				};
				goal.Id = Guid.Parse(fhirGoal.Reference.ToString().Replace("Goal/", ""));
				carePlan.Goals.Add(goal);
			}
		
		}
		return carePlan;
	}

}
