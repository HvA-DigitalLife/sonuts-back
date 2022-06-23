using System.Text;
using Sonuts.Infrastructure.Fhir.Adapters;
using Sonuts.Infrastructure.Fhir.Interfaces;
using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirInterventionPlanDao : IInterventionPlanDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirInterventionPlanDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<List<InterventionPlan>> SelectAllByParticipantId(string participantId)
	{

		// load and parse domains instance
		var client = _httpClientFactory.CreateClient("Fhir");
		var result = await client.GetStringAsync("PlanDefinition?type=" + participantId); // todo, add extension and search parameter for mib domain id valueset to FHIR, will get all of them for now

		return FhirInterventionPlanAdapter.FromJsonBundle(result);
	}

	public async Task<InterventionPlan> Insert(InterventionPlan interventionPlan)
	{
		var client = _httpClientFactory.CreateClient("Fhir");
		var response = await client.PostAsync("PlanDefinition", new StringContent(FhirInterventionPlanAdapter.ToJson(interventionPlan), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirInterventionPlanAdapter.FromJson(responseContent);
	}

	public async Task<bool> Update(InterventionPlan interventionPlan)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(int recommendationId)
	{
		await Task.Delay(1);
		return true;
	}
}
