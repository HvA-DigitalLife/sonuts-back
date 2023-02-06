using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirActivityAdapter
{

	public static List<Activity> FromJsonBundle(string json)
	{
		// create list of guidelines
		var activityList = new List<Activity>();

		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.ActivityDefinition)) {
				var activityDefinitionEntry = (Hl7.Fhir.Model.ActivityDefinition) fhirBundleEntry.Resource;
				// Convert Fhir plan definition object and add to list
				activityList.Add(FhirActivityDefinitionToActivity(activityDefinitionEntry));
			}
		}

		// return list of guidelines
		return activityList;
	}


	public static Activity FromJson (string json)
	{       
		var fhirJsonParser = new FhirJsonParser();
		// parse plan definition resource and return Guideline object
		return FhirActivityDefinitionToActivity(fhirJsonParser.Parse<Hl7.Fhir.Model.ActivityDefinition>(json));
	}

	public static string ToJson ( Activity activity )
	{
		// create plan definition and meta data
		var fhirActivityDefinition = new Hl7.Fhir.Model.ActivityDefinition {
			Id = activity.Id.ToString(), 
			Name = activity.Name,
			Description = new Hl7.Fhir.Model.Markdown(activity.Description)
		};

		if (activity.Image is not null) {
			fhirActivityDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/image-extension", Value = new Hl7.Fhir.Model.FhirString(activity.Image.Extension)
			});

			fhirActivityDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/image-name", Value = new Hl7.Fhir.Model.FhirString(activity.Image.Name)
			});
		}
		foreach (var activityVideo in activity.Videos) {
			fhirActivityDefinition.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extensions/PlanDefinition/video", Value = new Hl7.Fhir.Model.FhirString(activityVideo.Url)
			});
		}


          
		// serialize and return
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirActivityDefinition);
	}

	private static Activity FhirActivityDefinitionToActivity(Hl7.Fhir.Model.ActivityDefinition fhirActivityDefinition) {

		var activity = new Activity{
			Id = Guid.Parse(fhirActivityDefinition.Id),
			Name = fhirActivityDefinition.Title,
			Description = fhirActivityDefinition.Description?.ToString(),
			Image = new Image{
				Extension = "NA"
			}
		};

		// parse extensions
		foreach (var fhirActionExtension in fhirActivityDefinition.Extension)
		{
			if (fhirActionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/image-extension") {
				activity.Image.Extension = fhirActionExtension.Value.ToString() ?? "";
			}
			if (fhirActionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/image-name") {
				activity.Image.Name = fhirActionExtension.Value.ToString() ?? "";
			}
			if (fhirActionExtension.Url == "https://mibplatform.nl/fhir/Extensions/PlanDefinition/video") {
				activity.Videos.Add(new Video{Url = fhirActionExtension.Value.ToString() ?? ""});
			}
		}

		return activity;
	}

}
