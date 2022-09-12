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
			
			var fhirDayOfWeekCode = new Hl7.Fhir.Model.Code<Hl7.Fhir.Model.DaysOfWeek>();

			fhirDayOfWeekCode.Value = goal.Moment.Day switch
			{
				// set question type
				DayOfWeek.Monday => Hl7.Fhir.Model.DaysOfWeek.Mon,
				DayOfWeek.Tuesday => Hl7.Fhir.Model.DaysOfWeek.Tue,
				DayOfWeek.Wednesday => Hl7.Fhir.Model.DaysOfWeek.Fri,
				DayOfWeek.Thursday => Hl7.Fhir.Model.DaysOfWeek.Thu,
				DayOfWeek.Friday => Hl7.Fhir.Model.DaysOfWeek.Fri,
				DayOfWeek.Saturday => Hl7.Fhir.Model.DaysOfWeek.Sat,
				DayOfWeek.Sunday => Hl7.Fhir.Model.DaysOfWeek.Sun,
				_ => fhirDayOfWeekCode.Value
			};

			fhirScheduled.Repeat.DayOfWeekElement.Add(fhirDayOfWeekCode);
			fhirScheduled.Repeat.TimeOfDayElement.Add(new Hl7.Fhir.Model.Time(goal.Moment.Time.ToString()));


			fhirScheduled.Repeat.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extentions/Timing/MomentType", 
				Value = new Hl7.Fhir.Model.FhirString(goal.Moment.Type.ToString())
			});

			fhirScheduled.Repeat.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extentions/Timing/EventName", 
				Value = new Hl7.Fhir.Model.FhirString(goal.Moment.EventName)
			});

			// add schedule
			fhirActivity.Detail.Scheduled = fhirScheduled;


			fhirScheduled.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extentions/CarePlan/Activity/Reminder", 
				Value = new Hl7.Fhir.Model.FhirString(goal.Reminder.ToString())
			});



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
