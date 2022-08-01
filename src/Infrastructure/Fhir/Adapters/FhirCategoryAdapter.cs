using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirCategoryAdapter
{

	public static List<Category> FromJsonBundleToList(string json)
	{
		var categoryList = new List<Category>();
		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// retrieve the plan definitions
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.ValueSet)) {
				var valueSetEntry = (Hl7.Fhir.Model.ValueSet) fhirBundleEntry.Resource;
				// Convert Fhir plan definition object and add to list
				return FhirValueSetToCategoryList(valueSetEntry);
			}
		}

		// return list of guidelines
		return categoryList;
	}


	public static List<Category> FromJsonToList (string json)
	{       
		var fhirJsonParser = new FhirJsonParser();
		// parse plan definition resource and return Guideline object
		return FhirValueSetToCategoryList(fhirJsonParser.Parse<Hl7.Fhir.Model.ValueSet>(json));
	}

	public static List<Category> FhirValueSetToCategoryList(Hl7.Fhir.Model.ValueSet fhirValueSet)
	{
		var categoriesList = new List<Category>();
		// create questionnaire instance
            
		foreach (var fhirInclude in fhirValueSet.Compose.Include) {
			foreach (var fhirConcept in fhirInclude.Concept) {
				// todo: add color extension
				Category category = new(){Id = Guid.Parse(fhirConcept.Code), Name = fhirConcept.Display};
				foreach (var fhirConceptExtension in fhirConcept.Extension) {
					if (fhirConceptExtension.Url == "https://mibplatform.nl/fhir/Extentions/ValueSet/isActive") {
						if (fhirConceptExtension.Value is not null) {
							category.IsActive = bool.Parse(fhirConceptExtension.Value.ToString());
						} else {
							category.IsActive = false;
						}
					}
					if (fhirConceptExtension.Url == "https://mibplatform.nl/fhir/Extentions/ValueSet/color") {
						if (fhirConceptExtension.Value is not null) {
							category.Color = fhirConceptExtension.Value.ToString();
						}
					}
					
				}
				categoriesList.Add(category);
			}
		}
			
		

		return categoriesList;
	}

	public static string ToJson ( List<Category> categories )
	{
		// create plan definion and meta data
		var fhirValueSet= new Hl7.Fhir.Model.ValueSet();

		fhirValueSet.Extension.Add(new Hl7.Fhir.Model.Extension { 
			Url = "http://hl7.org/fhir/StructureDefinition/valueset-extensible", 
			Value = new Hl7.Fhir.Model.FhirBoolean(true)
		});

		// add identifier
		fhirValueSet.Identifier.Add(new Hl7.Fhir.Model.Identifier {
				System = "https://mibplatform.nl/fhir/Identifiers/Valueset",
				Value = "mib-categories"
			});

		fhirValueSet.Url = "https://mibplatform.nl/fhir/ValueSet/categories";
		fhirValueSet.Name = "MiB Categories";
		fhirValueSet.Status = Hl7.Fhir.Model.PublicationStatus.Draft;
		fhirValueSet.Experimental = true;
		fhirValueSet.Publisher = "MiB Platform";
		fhirValueSet.Description = new Hl7.Fhir.Model.Markdown("Categories which will be used in the MiB platorm.");
		fhirValueSet.Copyright = new Hl7.Fhir.Model.Markdown("© 2022 DigitalLife Hogeschool van Amsterdam.");
		fhirValueSet.Compose = new Hl7.Fhir.Model.ValueSet.ComposeComponent();

		// create categories container
		var fhirConceptSet = new Hl7.Fhir.Model.ValueSet.ConceptSetComponent {
			System = "https://mibplatform.nl/fhir/ValueSet/categories"
		};
		
		// loop trough categories and add
		foreach (var category in categories) {
			var fhirConceptReference = new Hl7.Fhir.Model.ValueSet.ConceptReferenceComponent{
				Code = category.Id.ToString(),
				Display = category.Name
			};
			fhirConceptReference.Extension.Add(new Hl7.Fhir.Model.Extension { 
				Url = "https://mibplatform.nl/fhir/Extentions/ValueSet/isActive", 
				Value = new Hl7.Fhir.Model.FhirBoolean(category.IsActive)
			});
			if (category.Color is not null) {
				fhirConceptReference.Extension.Add(new Hl7.Fhir.Model.Extension { 
					Url = "https://mibplatform.nl/fhir/Extentions/ValueSet/color", 
					Value = new Hl7.Fhir.Model.FhirString(category.Color)
				});
			}
			fhirConceptSet.Concept.Add(fhirConceptReference);
			
		}

		// write to categories container
		fhirValueSet.Compose.Include.Add(fhirConceptSet);

		// serialize and return
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirValueSet);
	}

}
