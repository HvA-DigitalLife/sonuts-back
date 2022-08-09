using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirCategoryDao : ICategoryDao
{

	private readonly IHttpClientFactory _httpClientFactory;
	private readonly FhirThemeDao _fhirThemeDao;

	public FhirCategoryDao(IHttpClientFactory httpClientFactory) {
		_httpClientFactory = httpClientFactory;
		_fhirThemeDao = new FhirThemeDao(_httpClientFactory);
	}



	public async Task<List<Category>> SelectAll()
	{
		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var categories = FhirCategoryAdapter.FromJsonBundleToList(await client.GetStringAsync("ValueSet/?identifier=mib-categories"));

		foreach (var category in categories) {
			category.Themes =  await _fhirThemeDao.SelectAllByCategoryId(category.Id.ToString());
		}

		return categories;
	}

	public async Task<List<Category>> Initialize(List<Category> categories)
	{
		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PostAsync("ValueSet", new StringContent(FhirCategoryAdapter.ToJson(categories), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		Console.WriteLine(responseContent);

		// todo make non bundle version
		return FhirCategoryAdapter.FromJsonToList(responseContent);
	}

}
