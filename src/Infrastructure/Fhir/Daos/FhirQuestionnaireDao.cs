using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirQuestionnaireDao : IQuestionnaireDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirQuestionnaireDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<Questionnaire> Select(string id)
	{

		// load and parse questionnaire instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("Questionnaire/" + id);

		return FhirQuestionnaireAdapter.FromJson(result);
	}

	public async Task<Questionnaire> Insert(Questionnaire questionnaire)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PostAsync("Questionnaire", new StringContent(FhirQuestionnaireAdapter.ToJson(questionnaire), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		Console.WriteLine(responseContent);
		return FhirQuestionnaireAdapter.FromJson(responseContent);
	}

	public async Task<bool> Update(Questionnaire questionnaire)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(int id)
	{
		await Task.Delay(1);
		return true;
	}
}
