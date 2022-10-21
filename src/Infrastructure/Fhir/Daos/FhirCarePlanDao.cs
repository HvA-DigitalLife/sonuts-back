using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;
public class FhirCarePlanDao : ICarePlanDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirCarePlanDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<List<CarePlan>> SelectAllByParticipantId(Guid participantId)
	{

		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("PlanDefinition?type=" + participantId.ToString()); // todo, add extension and search parameter for mib domain id valueset to FHIR, will get all of them for now

		return FhirCarePlanAdapter.FromJsonBundle(result);
	}

	public async Task<CarePlan> Insert(CarePlan carePlan)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PutAsync("PlanDefinition/" + carePlan.Id.ToString(), new StringContent(FhirCarePlanAdapter.ToJson(carePlan), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirCarePlanAdapter.FromJson(responseContent);
	}

	public async Task<bool> Update(CarePlan carePlan)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(Guid id)
	{
		await Task.Delay(1);
		return true;
	}
}
