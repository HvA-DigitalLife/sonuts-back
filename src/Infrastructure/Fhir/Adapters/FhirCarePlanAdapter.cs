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
		var fhirCarePlan= new Hl7.Fhir.Model.CarePlan();

		// add identifier
		fhirCarePlan.Identifier.Add(new Hl7.Fhir.Model.Identifier {
				System = "https://mibplatform.nl/fhir/mib/identifier",
				Value = carePlan.Id.ToString()
			});

		fhirCarePlan.Period =  new Hl7.Fhir.Model.Period {
			Start = carePlan.Start.ToString(),
			End = carePlan.End.ToString()
		};


		foreach (var goal in carePlan.Goals) {
			var fhirActivity = new Hl7.Fhir.Model.CarePlan.ActivityComponent();
			fhirActivity.Detail.DailyAmount = new Hl7.Fhir.Model.Quantity{Value = goal.FrequencyAmount};

			var fhirScheduled = new Hl7.Fhir.Model.Timing();
		    // add moment type extension
			// add moment eventname extension

			fhirScheduled.Repeat.TimeOfDayElement.Add(new Hl7.Fhir.Model.Time(goal.Moment.Time.ToString()));
			fhirActivity.Detail.Scheduled = fhirScheduled;

			// reminder is extension

			// execution is observation stored in outcomeReference

		}

          
		// serialize and return
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirCarePlan);
	}

	private static CarePlan FhirCarePlanToCarePlan(Hl7.Fhir.Model.CarePlan fhirCarePlan) {
		// create interventionPlan model and add meta data
		var carePlan = new CarePlan();

		foreach (var fhirId in fhirCarePlan.Identifier) {
			if (fhirId.System == "https://mibplatform.nl/fhir/mib/identifier") {
				carePlan.Id =  Guid.Parse(fhirId.Value);
			}
		}

		carePlan.Start = DateOnly.Parse(fhirCarePlan.Period.Start);
		carePlan.End = DateOnly.Parse(fhirCarePlan.Period.End);

    
        
		// foreach (var action in planDefinition.Action) {
		//     // create goal and meta data
		//     var goal = new Goal();
		//     goal.Title = action.Title;
		//     goal.Text = action.Description;

		//     foreach (var dataRequirement in action.Input) {
		//         // create data field
		//         var dataField = new GoalDataField();

		//         // parse title, defaultValue and unit extensions
		//         foreach (var dataRequirementExtension in dataRequirement.Extension) {
		//             if (dataRequirementExtension.Url == "https://mibplatform.nl/fhir/extensions/DataRequirement/title") {
		//                 dataField.Title = dataRequirementExtension.Value.ToString();
		//             }
		//             if (dataRequirementExtension.Url == "https://mibplatform.nl/fhir/extensions/DataRequirement/defaultValue") {
		//                 dataField.DefaultValue = dataRequirementExtension.Value.ToString();
		//             }
		//             if (dataRequirementExtension.Url == "https://mibplatform.nl/fhir/extensions/DataRequirement/unit") {
		//                 dataField.DataType = dataRequirementExtension.Value.ToString();
		//             }
		//         }
		//         // add datafields to goal
		//         goal.DataFields.Add(dataField);
		//     }
		//     // add goal to recommendation
		//     recommendation.Goals.Add(goal);
		// }
		return carePlan;
	}

}
