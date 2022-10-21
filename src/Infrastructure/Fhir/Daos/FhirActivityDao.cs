using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirActivityDao : IActivityDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirActivityDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<Activity> Select(Guid id)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("PlanDefinition/" + id.ToString());

		return FhirActivityAdapter.FromJson(result);
	}

	public async Task<Activity> Insert(Activity activity)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PutAsync("ActivityDefinition/" + activity.Id.ToString(), new StringContent(FhirActivityAdapter.ToJson(activity), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirActivityAdapter.FromJson(responseContent);
	}

	public async Task<Activity> Update(Activity activity)
	{
		await Task.Delay(1);
		return activity;
	}

	public async Task<bool> Delete(Guid id)
	{
		await Task.Delay(1);
		return true;
	}
}