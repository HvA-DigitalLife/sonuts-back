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
		var fhirPlanDefinition = new Hl7.Fhir.Model.PlanDefinition {
			Id = theme.Id.ToString()
		};


		fhirPlanDefinition.Title = theme.Name;
		fhirPlanDefinition.Description = new Hl7.Fhir.Model.Markdown(theme.Description);

		// add domain type coding
		var fhirTypeCodableConcept = new Hl7.Fhir.Model.CodeableConcept();
		var fhirTypeCoding = new Hl7.Fhir.Model.Coding();

		fhirTypeCoding.System = "https://mibplatform.nl/fhir/ValueSet/categories";
		fhirTypeCoding.Code = theme.Category.Id.ToString();
		fhirTypeCoding.Display = theme.Category.Name;
		fhirTypeCodableConcept.Coding.Add(fhirTypeCoding);
		fhirPlanDefinition.Type = fhirTypeCodableConcept;


		fhirPlanDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/frequencyType", Value = new Hl7.Fhir.Model.FhirString(theme.FrequencyType.ToString())
		});

		fhirPlanDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/frequencyGoal", Value = new Hl7.Fhir.Model.Integer(theme.FrequencyGoal)
		});

		fhirPlanDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/currentQuestion", Value = new Hl7.Fhir.Model.FhirString(theme.CurrentFrequencyQuestion)
		});

		fhirPlanDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/goalQuestion", Value = new Hl7.Fhir.Model.FhirString(theme.GoalFrequencyQuestion)
		});

		foreach (var activity in theme.Activities) {
			// create action
			var fhirAction = new Hl7.Fhir.Model.PlanDefinition.ActionComponent();
			
			// add identifyer
			fhirAction.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/mib/identifier", Value = new Hl7.Fhir.Model.FhirString(activity.Id.ToString())
			});

			fhirAction.Title = activity.Name;
			fhirAction.Description = activity.Description;

			/*fhirAction.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/video", Value = new Hl7.Fhir.Model.FhirString(activity.Video)
			});*/

			// add action to plan definition
			fhirPlanDefinition.Action.Add(fhirAction);
		}
          
		// serialize and return
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirPlanDefinition);
	}

	private static Theme FhirPlanDefinitionToTheme(Hl7.Fhir.Model.PlanDefinition fhirPlanDefinition) {
		// create guideline model and add meta data
		var theme = new Theme{
			Id = Guid.Parse(fhirPlanDefinition.Id),
			Name = fhirPlanDefinition.Title,
			Description = fhirPlanDefinition.Description.ToString() ?? string.Empty,
			Image = new Image {
				Extension = "NA"
			},
			FrequencyType = FrequencyType.Amount, // to-do implement
			CurrentFrequencyQuestion = "",
			CurrentActivityQuestion = "",
			GoalFrequencyQuestion = ""
		};
		
        

		foreach (var fhirId in fhirPlanDefinition.Identifier) {
			if (fhirId.System == "https://mibplatform.nl/fhir/mib/identifier") {
				theme.Id =  Guid.Parse(fhirId.Value);
			}
		}

		// read domain coding
		foreach (var typeCoding in fhirPlanDefinition.Type.Coding) {
			if (typeCoding.System == "https://mibplatform.nl/fhir/ValueSet/domains") {
				theme.Category = new Category {
					Id = Guid.Parse(typeCoding.Code),
					Name = "",
					Color = "",
					Questionnaire = new Questionnaire{
						Title = ""
					},					
				};
			}
		}

		foreach (var fhirPlanDefinitionExtension in fhirPlanDefinition.Extension)
		{
			if (fhirPlanDefinitionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/frequencyType") {
				theme.FrequencyType = (FrequencyType) Enum.Parse(typeof(FrequencyType), fhirPlanDefinitionExtension.Value.ToString() ?? string.Empty);
			}
			if (fhirPlanDefinitionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/frequencyGoal") {
				theme.FrequencyGoal = int.Parse(fhirPlanDefinitionExtension.Value.ToString() ?? string.Empty);
			}
			if (fhirPlanDefinitionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/currentQuestion") {
				theme.CurrentFrequencyQuestion = fhirPlanDefinitionExtension.Value.ToString() ?? string.Empty;
			}
			if (fhirPlanDefinitionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/goalQuestion") {
				theme.GoalFrequencyQuestion = fhirPlanDefinitionExtension.Value.ToString() ?? string.Empty;
			}
		}


        // TODO: create references to activities
		foreach (var fhirAction in fhirPlanDefinition.Action) {
			// create goal and meta data
			var activity = new Activity{
				Name = fhirAction.Title,
				Description = fhirAction.Description,
				Image = new Image{
					Extension = "NA"
				}
			};
			

			// parse identifier
			foreach (var fhirActionExtension in fhirAction.Extension)
			{
				/*if (fhirActionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/video") {
					activity.Video = fhirActionExtension.Value.ToString();
				}
				if (fhirActionExtension.Url == "https://mibplatform.nl/fhir/mib/identifier")
				{
					activity.Id = Guid.Parse(fhirActionExtension.Value.ToString());
				}*/
			}

			// add goal to guideline
			theme.Activities.Add(activity);
		}

		return theme;
	}

}
