using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirCategoryAdapter
{
	public static List<Category> FromJsonBundle(string json)
	{
		var categoriesList = new List<Category>();
		// create questionnaire instance
            
		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = fhirJsonParser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var fhirBundleEntry in fhirBundle.Entry) {
			// parse the valueset
			if (fhirBundleEntry.Resource.GetType() == typeof(Hl7.Fhir.Model.ValueSet)) {
				var fhirValueSetEntry = (Hl7.Fhir.Model.ValueSet) fhirBundleEntry.Resource;
				foreach (var fhirInclude in fhirValueSetEntry.Compose.Include) {
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
							if (fhirConceptExtension.Url == "https://mibplatform.nl/fhir/Extentions/ValueSet/isActive") {
								category.IsActive = bool.Parse(fhirConceptExtension.Value.ToString());
							}
							
						}
						categoriesList.Add(category);
					}
				}
			}
		}

		return categoriesList;
	}
}
