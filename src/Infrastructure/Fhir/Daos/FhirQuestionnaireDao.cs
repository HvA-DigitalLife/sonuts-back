using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirQuestionnaireDao : IQuestionnaireDao
{

	private readonly IHttpClientFactory _httpClientFactory;
	private readonly ICategoryDao _categoryDao;

	public FhirQuestionnaireDao(IHttpClientFactory httpClientFactory, ICategoryDao categoryDao) {
		_httpClientFactory = httpClientFactory;
		_categoryDao = categoryDao;
	}


	public async Task<Questionnaire> SelectByCategoryId(System.Guid id)
	{

		var category = _categoryDao.Select(id);

		// load and parse questionnaire instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("Questionnaire/?identifier=" + category.Id.ToString());
		Console.WriteLine(result);

		return FhirQuestionnaireAdapter.FromJson(result);
	}

	public async Task<Questionnaire> Select(System.Guid id)
	{

		// load and parse questionnaire instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("Questionnaire/?identifier=" + id.ToString());
		Console.WriteLine(result);

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

}
