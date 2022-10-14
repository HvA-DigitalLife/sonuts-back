using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirGoalDao : IGoalDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirGoalDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<Goal> Select(System.Guid id)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("Goal/" + id.ToString());

		return FhirGoalAdapter.FromJson(result);
	}

	public async Task<Goal> Insert(Goal goal)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PutAsync("Goal/" + goal.Id.ToString(), new StringContent(FhirGoalAdapter.ToJson(goal), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirGoalAdapter.FromJson(responseContent);
	}

	public async Task<Goal> Update(Goal goal)
	{
		await Task.Delay(1);
		return goal;
	}

	public async Task<bool> Delete(System.Guid id)
	{
		await Task.Delay(1);
		return true;
	}
}