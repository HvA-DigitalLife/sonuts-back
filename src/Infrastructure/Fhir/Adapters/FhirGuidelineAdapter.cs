using Hl7.Fhir.Serialization;
using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirGuidelineAdapter
{

	public static List<Guideline> FromJsonBundle(string json)
	{
		// create list of guidelines
		var guidelineList = new List<Guideline>();

		// create fhir parser
		var parser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = parser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var entry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (entry.Resource.GetType() == typeof(Hl7.Fhir.Model.PlanDefinition)) {
				var pdEntry = (Hl7.Fhir.Model.PlanDefinition) entry.Resource;
				// Convert Fhir plan definition object and add to list
				guidelineList.Add(FhirPlanDefinitionToGuideline(pdEntry));
			}
		}

		// return list of guidelines
		return guidelineList;
	}




	public static Guideline FromJson (string json)
	{       
		var parser = new FhirJsonParser();
		// parse plan definition resource and return Guideline object
		return FhirPlanDefinitionToGuideline(parser.Parse<Hl7.Fhir.Model.PlanDefinition>(json));
	}

	public static string ToJson ( Guideline guideline )
	{
		// create plan definion and meta data
		var fhirPlanDefinition= new Hl7.Fhir.Model.PlanDefinition();
		fhirPlanDefinition.Title = guideline.Title;
		fhirPlanDefinition.Description = new Hl7.Fhir.Model.Markdown(guideline.Text);

		// add domain type coding
		var fhirTypeConcept = new Hl7.Fhir.Model.CodeableConcept();
		var fhirTypeCoding = new Hl7.Fhir.Model.Coding();
		fhirTypeCoding.System = "https://mibplatform.nl/fhir/ValueSet/domains";
		fhirTypeCoding.Code = guideline.DomainId;
		fhirTypeConcept.Coding.Add(fhirTypeCoding);
		fhirPlanDefinition.Type = fhirTypeConcept;

		foreach (var goal in guideline.Goals) {
			// create action
			var action = new Hl7.Fhir.Model.PlanDefinition.ActionComponent();
			action.Title = goal.Title;
			action.Description = goal.Text;

			foreach (var dataField in goal.DataFields) {
				// create data requirement
				var dataRequirement = new Hl7.Fhir.Model.DataRequirement();

				// add of type string (will have more datatypes once defined)
				if (dataField.DataType == "string") {
					dataRequirement.Type = Hl7.Fhir.Model.FHIRAllTypes.String;
				}
                
				// title extension
				var titleExtension = new Hl7.Fhir.Model.Extension();
				titleExtension.Url = "https://mibplatform.nl/fhir/extensions/DataRequirement/title";
				titleExtension.Value = new Hl7.Fhir.Model.FhirString(dataField.Title);
				dataRequirement.Extension.Add(titleExtension);

				// default value extension
				var defaultValueExtension = new Hl7.Fhir.Model.Extension();
				defaultValueExtension.Url = "https://mibplatform.nl/fhir/extensions/DataRequirement/defaultValue";
				defaultValueExtension.Value = new Hl7.Fhir.Model.FhirString(dataField.DefaultValue);
				dataRequirement.Extension.Add(defaultValueExtension);

				// unit extension
				var unitExtension = new Hl7.Fhir.Model.Extension();
				unitExtension.Url = "https://mibplatform.nl/fhir/extensions/DataRequirement/unit";
				unitExtension.Value = new Hl7.Fhir.Model.FhirString(dataField.DataType);
				dataRequirement.Extension.Add(unitExtension);

				// add requirement to input list
				action.Input.Add(dataRequirement);
			}
			// add action to plan definition
			fhirPlanDefinition.Action.Add(action);
		}
          
		// serialize and return
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirPlanDefinition);
	}

	private static Guideline FhirPlanDefinitionToGuideline(Hl7.Fhir.Model.PlanDefinition planDefinition) {
		// create guideline model and add meta data
		var guideline = new Guideline();
		guideline.Id = planDefinition.Id;
		guideline.Title = planDefinition.Title;
		guideline.Text = planDefinition.Description.ToString();
        
		// read domain coding
		foreach (var typeCoding in planDefinition.Type.Coding) {
			if (typeCoding.System == "https://mibplatform.nl/fhir/ValueSet/domains") {
				guideline.DomainId = typeCoding.Code;
			}
		}
        
		foreach (var action in planDefinition.Action) {
			// create goal and meta data
			var goal = new Goal();
			goal.Title = action.Title;
			goal.Text = action.Description;

			foreach (var dataRequirement in action.Input) {
				// create data field
				var dataField = new GoalDataField();

				// parse title, defaultValue and unit extensions
				foreach (var dataRequirementExtension in dataRequirement.Extension) {
					if (dataRequirementExtension.Url == "https://mibplatform.nl/fhir/extensions/DataRequirement/title") {
						dataField.Title = dataRequirementExtension.Value.ToString();
					}
					if (dataRequirementExtension.Url == "https://mibplatform.nl/fhir/extensions/DataRequirement/defaultValue") {
						dataField.DefaultValue = dataRequirementExtension.Value.ToString();
					}
					if (dataRequirementExtension.Url == "https://mibplatform.nl/fhir/extensions/DataRequirement/unit") {
						dataField.DataType = dataRequirementExtension.Value.ToString();
					}
				}
				// add datafields to goal
				goal.DataFields.Add(dataField);
			}
			// add goal to guideline
			guideline.Goals.Add(goal);
		}
		// return object
		return guideline;
	}

}
