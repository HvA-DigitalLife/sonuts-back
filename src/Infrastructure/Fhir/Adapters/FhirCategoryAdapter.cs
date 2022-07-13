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
		var parser = new FhirJsonParser();

		// parse as bundle
		var fhirBundle = parser.Parse<Hl7.Fhir.Model.Bundle>(json);

		foreach (var entry in fhirBundle.Entry) {
			// parse the valueset
			if (entry.Resource.GetType() == typeof(Hl7.Fhir.Model.ValueSet)) {
				var vsEntry = (Hl7.Fhir.Model.ValueSet) entry.Resource;
				foreach (var include in vsEntry.Compose.Include) {
					foreach (var concept in include.Concept) {
						Category domain = new(){Id = Guid.Parse(concept.Code), Name = concept.Display, Color = "blue"};
						categoriesList.Add(domain);
					}
				}
			}
		}

		return categoriesList;
	}
}
