using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirCategoryDao : ICategoryDao
{

	private readonly IHttpClientFactory _httpClientFactory;
	private readonly IThemeDao _themeDao;

	public FhirCategoryDao(IHttpClientFactory httpClientFactory, IThemeDao themeDao) {
		_httpClientFactory = httpClientFactory;
		_themeDao = themeDao;
	}

	public async Task<Category> Select(Guid id)
	{
		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var categories = FhirCategoryAdapter.FromJsonToList(await client.GetStringAsync("ValueSet/mib-categories"));
		var selectedCategory = new Category();
		foreach (var category in categories) {
			if (category.Id == id) {
				category.Themes =  await _themeDao.SelectAllByCategoryId(category.Id);
				selectedCategory = category;
			}
			
		}

		return selectedCategory;
	}

	public async Task<List<Category>> SelectAll()
	{
		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var categories = FhirCategoryAdapter.FromJsonToList(await client.GetStringAsync("ValueSet/mib-categories"));

		foreach (var category in categories) {
			category.Themes =  await _themeDao.SelectAllByCategoryId(category.Id);
		}

		return categories;
	}

	public async Task<List<Category>> Initialize(List<Category> categories)
	{
		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PutAsync("ValueSet/mib-categories", new StringContent(FhirCategoryAdapter.ToJson(categories), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		// todo make non bundle version
		return FhirCategoryAdapter.FromJsonToList(responseContent);
	}

}
