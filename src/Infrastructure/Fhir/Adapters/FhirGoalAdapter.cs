using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirGoalAdapter
{

	public static List<Goal> FromJsonBundle(string json)
	{
		// create list of recommendations
		var goalList = new List<Goal>();

		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.Goal)) {
				var goalEntry = (Hl7.Fhir.Model.Goal) fhirBundleEntry.Resource;
				// Convert Fhir plan definition object and add to list
				goalList.Add(FhirGoalToGoal(goalEntry));
			}
		}

		// return list of recommendations
		return goalList;
	}


	public static Goal FromJson (string json)
	{       
		var fhirJsonParser = new FhirJsonParser();
		// parse plan definition resource and return Recommendation object
		return FhirGoalToGoal(fhirJsonParser.Parse<Hl7.Fhir.Model.Goal>(json));
	}

	public static string ToJson ( Goal goal )
	{
		// create plan definion and meta data
		var fhirGoal= new Hl7.Fhir.Model.Goal{
			Id = goal.Id.ToString(),
			LifecycleStatus = Hl7.Fhir.Model.Goal.GoalLifecycleStatus.Active
		};
		// add custom name
		fhirGoal.Description = new Hl7.Fhir.Model.CodeableConcept();
		fhirGoal.Description.Coding.Add(new Hl7.Fhir.Model.Coding{ Code = "custom-name", Display = goal.CustomName });
		
		fhirGoal.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extensions/Goal/ActivityDefinitionReference", Value = new Hl7.Fhir.Model.ResourceReference{ Reference = "ActivityDefinition/" + goal.Activity.Id.ToString() }
		});

		var fhirGoalTarget = new Hl7.Fhir.Model.Goal.TargetComponent();
		fhirGoalTarget.Measure = new Hl7.Fhir.Model.CodeableConcept();
		fhirGoalTarget.Measure.Coding.Add(new Hl7.Fhir.Model.Coding{ Code = "default-measure", Display = "default-measure" });

		fhirGoalTarget.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extentions/Goal/Moment/Day", 
			Value = new Hl7.Fhir.Model.FhirString(goal.Moment.Day.ToString())
		});

		fhirGoalTarget.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extentions/Goal/Moment/Time", 
			Value = new Hl7.Fhir.Model.Time(goal.Moment.Time.ToString())
		});
		fhirGoalTarget.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extentions/Goal/Moment/Type", 
			Value = new Hl7.Fhir.Model.FhirString(goal.Moment.Type.ToString())
		});

		fhirGoalTarget.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extentions/Goal/Moment/EventName", 
			Value = new Hl7.Fhir.Model.FhirString(goal.Moment.EventName is not null?goal.Moment.EventName.ToString():"")
		});

		fhirGoalTarget.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extentions/Goal/FrequencyAmount", 
			Value = new Hl7.Fhir.Model.Integer(goal.FrequencyAmount)
		});

		goal.FrequencyAmount.ToString();
	

		// serialize and return
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirGoal);
	}

	private static Goal FhirGoalToGoal(Hl7.Fhir.Model.Goal fhirGoal) {

		var goal = new Goal{
			Id = Guid.Parse(fhirGoal.Id), 
			CustomName = fhirGoal.Description.Coding.First().Display
		};


		foreach (var fhirGoalExtension in fhirGoal.Extension)
			{
				if (fhirGoalExtension.Url == "https://mibplatform.nl/fhir/Extensions/Goal/ActivityDefinitionReference") {
					Hl7.Fhir.Model.ResourceReference fhirGoalActivityDefinitionReference = (Hl7.Fhir.Model.ResourceReference) fhirGoalExtension.Value;
					goal.Activity.Id = Guid.Parse(fhirGoalActivityDefinitionReference.Reference.Replace("ActivityDefinition/", ""));
				}
			}

		var fhirGoalTarget = fhirGoal.Target.First();

		foreach (var fhirGoalTargetExtension in fhirGoalTarget.Extension)
			{
				if (fhirGoalTargetExtension.Url == "https://mibplatform.nl/fhir/Extentions/Goal/Moment/Day") {
					goal.Moment.Day = (DayOfWeek) Enum.Parse(typeof(DayOfWeek), fhirGoalTargetExtension.Value.ToString() ?? "Sunday");
				}
				if (fhirGoalTargetExtension.Url == "https://mibplatform.nl/fhir/Extentions/Goal/Moment/Time") {
					goal.Moment.Time = TimeOnly.Parse(fhirGoalTargetExtension.Value.ToString() ?? "");
					
				}
				if (fhirGoalTargetExtension.Url == "https://mibplatform.nl/fhir/Extentions/Goal/Moment/Type") {
					goal.Moment.Type = (MomentType) Enum.Parse(typeof(MomentType), fhirGoalTargetExtension.Value.ToString() ?? "Before");
				}
				if (fhirGoalTargetExtension.Url == "https://mibplatform.nl/fhir/Extentions/Goal/Moment/EventName") {
					goal.Moment.EventName = fhirGoalTargetExtension.Value.ToString();
				}
				if (fhirGoalTargetExtension.Url == "https://mibplatform.nl/fhir/Extentions/Goal/FrequencyAmount") {
					Hl7.Fhir.Model.Integer fhirGoalFrequencyAmount = (Hl7.Fhir.Model.Integer) fhirGoalTargetExtension.Value;
					goal.FrequencyAmount = fhirGoalFrequencyAmount.Value.HasValue?((int)fhirGoalFrequencyAmount.Value):0;
				}
			}



	
		return goal;
	}

}
