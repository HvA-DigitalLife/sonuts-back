using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;
using Sonuts.Infrastructure.Fhir.Interfaces;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirDomainDao : IDomainDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirDomainDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<List<Models.Domain>> SelectAll()
	{
		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("ValueSet/?identifier=mib-domains");

		return FhirDomainAdapter.FromJsonBundle(result);
	}

	public async Task<Models.Domain> Insert(Models.Domain domain)
	{
		await Task.Delay(1);
		return new Models.Domain();
	}

	public async Task<bool> Update(Models.Domain participant)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(int participantId)
	{
		await Task.Delay(1);
		return true;
	}
}
