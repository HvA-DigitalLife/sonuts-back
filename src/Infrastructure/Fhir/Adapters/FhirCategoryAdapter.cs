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
						Category category = new(){Id = Guid.Parse(fhirConcept.Code), Name = fhirConcept.Display, Color = "blue"};
						categoriesList.Add(category);
					}
				}
			}
		}

		return categoriesList;
	}
}
