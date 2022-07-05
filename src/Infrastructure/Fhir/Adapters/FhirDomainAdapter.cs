using Hl7.Fhir.Serialization;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirDomainAdapter
{
	public static List<Models.Domain> FromJsonBundle(string json)
	{
		var domainsList = new List<Models.Domain>();
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
						Models.Domain domain = new(){Id = concept.Code, Name = concept.Display};
						domainsList.Add(domain);
					}
				}
			}
		}

		return domainsList;
	}
}
