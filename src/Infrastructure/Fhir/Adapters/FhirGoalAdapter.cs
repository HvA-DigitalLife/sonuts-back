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
		var fhirGoal= new Hl7.Fhir.Model.Goal();

		// add identifier
		fhirGoal.Identifier.Add(new Hl7.Fhir.Model.Identifier {
				System = "https://mibplatform.nl/fhir/mib/identifier",
				Value = goal.Id.ToString()
			});

		var fhirTarget = new Hl7.Fhir.Model.Goal.TargetComponent();
		// todo check how to parse date
		//fhirTarget.Due = Hl7.Fhir.Model.Date(goal.Moment.Time);
		// todo add measure







		// foreach (var goal in recommendation.Goals) {
		// 	// create action
		// 	var action = new Hl7.Fhir.Model.PlanDefinition.ActionComponent();
		// 	action.Title = goal.Title;
		// 	action.Description = goal.Text;

		// 	foreach (var dataField in goal.DataFields) {
		// 	// create data requirement
		// 	var dataRequirement = new Hl7.Fhir.Model.DataRequirement();

		// 	// add of type string (will have more datatypes once defined)
		// 	if (dataField.DataType == "string") {
		// 		dataRequirement.Type = Hl7.Fhir.Model.FHIRAllTypes.String;
		// 	}
			
		// 	// title extension
		// 	var titleExtension = new Hl7.Fhir.Model.Extension();
		// 	titleExtension.Url = "https://mibplatform.nl/fhir/extensions/DataRequirement/title";
		// 	titleExtension.Value = new Hl7.Fhir.Model.FhirString(dataField.Title);
		// 	dataRequirement.Extension.Add(titleExtension);

		// 	// default value extension
		// 	var defaultValueExtension = new Hl7.Fhir.Model.Extension();
		// 	defaultValueExtension.Url = "https://mibplatform.nl/fhir/extensions/DataRequirement/defaultValue";
		// 	defaultValueExtension.Value = new Hl7.Fhir.Model.FhirString(dataField.DefaultValue);
		// 	dataRequirement.Extension.Add(defaultValueExtension);

		// 	// unit extension
		// 	var unitExtension = new Hl7.Fhir.Model.Extension();
		// 	unitExtension.Url = "https://mibplatform.nl/fhir/extensions/DataRequirement/unit";
		// 	unitExtension.Value = new Hl7.Fhir.Model.FhirString(dataField.DataType);
		// 	dataRequirement.Extension.Add(unitExtension);

		// 	// add requirement to input list
		// 	action.Input.Add(dataRequirement);
		// 	}
		// 	// add action to plan definition
		// 	fhirPlanDefinition.Action.Add(action);
		// }
          
		// serialize and return
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirGoal);
	}

	private static Goal FhirGoalToGoal(Hl7.Fhir.Model.Goal fhirGoal) {
		// create interventionPlan model and add meta data
		var goal = new Goal();

		foreach (var fhirId in fhirGoal.Identifier) {
			if (fhirId.System == "https://mibplatform.nl/fhir/mib/identifier") {
				goal.Id =  Guid.Parse(fhirId.Value);
			}
		}

		// todo parse target
    
        
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
		return goal;
	}

}
