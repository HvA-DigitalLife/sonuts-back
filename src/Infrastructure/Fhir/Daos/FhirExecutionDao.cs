using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirExecutionDao : IExecutionDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirExecutionDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<Execution> Select(Guid id)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("Observation/" + id.ToString());

		return FhirExecutionAdapter.FromJson(result);
	}

	public async Task<Execution> Insert(Execution goal)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PutAsync("Observation/" + goal.Id.ToString(), new StringContent(FhirExecutionAdapter.ToJson(goal), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirExecutionAdapter.FromJson(responseContent);
	}

	public async Task<Execution> Update(Execution execution)
	{
		await Task.Delay(1);
		return execution;
	}

	public async Task<bool> Delete(Guid id)
	{
		await Task.Delay(1);
		return true;
	}
}