using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirThemeAdapter
{

	public static List<Theme> FromJsonBundle(string json)
	{
		// create list of guidelines
		var themeList = new List<Theme>();

		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.PlanDefinition)) {
				var planDefinitionEntry = (Hl7.Fhir.Model.PlanDefinition) fhirBundleEntry.Resource;
				// Convert Fhir plan definition object and add to list
				themeList.Add(FhirPlanDefinitionToTheme(planDefinitionEntry));
			}
		}

		// return list of guidelines
		return themeList;
	}


	public static Theme FromJson (string json)
	{       
		var fhirJsonParser = new FhirJsonParser();
		// parse plan definition resource and return Guideline object
		return FhirPlanDefinitionToTheme(fhirJsonParser.Parse<Hl7.Fhir.Model.PlanDefinition>(json));
	}

	public static string ToJson ( Theme theme )
	{
		// create plan definion and meta data
		var fhirPlanDefinition= new Hl7.Fhir.Model.PlanDefinition();
		fhirPlanDefinition.Title = theme.Name;
		fhirPlanDefinition.Description = new Hl7.Fhir.Model.Markdown(theme.Description);

		// add domain type coding
		var fhirTypeConcept = new Hl7.Fhir.Model.CodeableConcept();
		var fhirTypeCoding = new Hl7.Fhir.Model.Coding();

		fhirTypeCoding.System = "https://mibplatform.nl/fhir/ValueSet/categories";
		fhirTypeCoding.Code = theme.Category.Id.ToString();
		fhirTypeCoding.Display = theme.Category.Name;
		fhirTypeConcept.Coding.Add(fhirTypeCoding);
		fhirPlanDefinition.Type = fhirTypeConcept;

		foreach (var activity in theme.Activities) {
			// create action
			var fhirAction = new Hl7.Fhir.Model.PlanDefinition.ActionComponent();
			fhirAction.Title = activity.Name;
			fhirAction.Description = activity.Description;

			// add action to plan definition
			fhirPlanDefinition.Action.Add(fhirAction);
		}
          
		// serialize and return
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirPlanDefinition);
	}

	private static Theme FhirPlanDefinitionToTheme(Hl7.Fhir.Model.PlanDefinition fhirPlanDefinition) {
		// create guideline model and add meta data
		var theme = new Theme();
		theme.Id = Guid.Parse(fhirPlanDefinition.Id);
		theme.Name = fhirPlanDefinition.Title;
		theme.Description = fhirPlanDefinition.Description.ToString();
        
		// read domain coding
		foreach (var typeCoding in fhirPlanDefinition.Type.Coding) {
			if (typeCoding.System == "https://mibplatform.nl/fhir/ValueSet/domains") {
				theme.Category.Id = Guid.Parse(typeCoding.Code);
			}
		}
        
		foreach (var fhirAction in fhirPlanDefinition.Action) {
			// create goal and meta data
			var activity = new Activity();
			activity.Name = fhirAction.Title;
			activity.Description = fhirAction.Description;

			// add goal to guideline
			theme.Activities.Add(activity);
		}

		return theme;
	}

}
