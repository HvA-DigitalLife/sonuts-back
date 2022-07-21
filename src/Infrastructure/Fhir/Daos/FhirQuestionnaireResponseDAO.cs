using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirQuestionnaireResponseDao : IQuestionnaireResponseDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirQuestionnaireResponseDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<QuestionnaireResponse> Select(string id)
	{
		// load and parse questionnaire response instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("QuestionnaireResponse/" + id);

		return FhirQuestionnaireResponseAdapter.FromJson(result);
	}

	public async Task<QuestionnaireResponse> Insert(QuestionnaireResponse questionnaireResponse)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PostAsync("QuestionnaireResponse", new StringContent(FhirQuestionnaireResponseAdapter.ToJson(questionnaireResponse), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirQuestionnaireResponseAdapter.FromJson(responseContent);
	}

	public async Task<bool> Update(QuestionnaireResponse questionnaireResponse)
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
