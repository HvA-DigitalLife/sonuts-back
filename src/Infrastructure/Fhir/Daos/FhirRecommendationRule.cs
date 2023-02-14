using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirRecommendationRule : IRecommendationRuleDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirRecommendationRule(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<RecommendationRule> Select(Guid id)
	{
		// var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		// var result = await client.GetStringAsync("Goal/" + id.ToString());

		// return FhirGoalAdapter.FromJson(result);
		return new RecommendationRule{ Value = ""};
	}

	public async Task<RecommendationRule> Insert(RecommendationRule recommendationRule)
	{
		// var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		// var response = await client.PutAsync("Goal/" + goal.Id.ToString(), new StringContent(FhirGoalAdapter.ToJson(goal), Encoding.UTF8, "application/json"));

		// var responseContent = await response.Content.ReadAsStringAsync();

		// return FhirGoalAdapter.FromJson(responseContent);
		return new RecommendationRule{ Value = ""};
	}

	public async Task<RecommendationRule> Update(RecommendationRule recommendationRule)
	{
		// var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		// var response = await client.PutAsync("Goal/" + goal.Id.ToString(), new StringContent(FhirGoalAdapter.ToJson(goal), Encoding.UTF8, "application/json"));

		// var responseContent = await response.Content.ReadAsStringAsync();

		// return FhirGoalAdapter.FromJson(responseContent);
		return new RecommendationRule{ Value = ""};
	}

	public async Task<bool> Delete(Guid id)
	{
		await Task.Delay(1);
		return true;
	}
}