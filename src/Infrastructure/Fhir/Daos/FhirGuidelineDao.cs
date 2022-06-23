using System.Text;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;
using Sonuts.Infrastructure.Fhir.Interfaces;
using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirGuidelineDao : IGuidelineDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirGuidelineDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<List<Guideline>> SelectAllByDomainId(string domainId)
	{

		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("PlanDefinition?type=" + domainId); // todo, add extension and search parameter for mib domain id valueset to FHIR, will get all of them for now

		return FhirGuidelineAdapter.FromJsonBundle(result);
	}

	public async Task<Guideline> Insert(Guideline guideline)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PostAsync("PlanDefinition", new StringContent(FhirGuidelineAdapter.ToJson(guideline), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirGuidelineAdapter.FromJson(responseContent);
	}

	public async Task<bool> Update(Guideline guideline)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(int guidelineId)
	{
		await Task.Delay(1);
		return true;
	}
}
