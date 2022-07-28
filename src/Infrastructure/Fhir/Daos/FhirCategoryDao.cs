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
		var categories = FhirCategoryAdapter.FromJsonBundle(await client.GetStringAsync("ValueSet/?identifier=mib-categories"));

		foreach (var category in categories) {
			category.Themes =  await _fhirThemeDao.SelectAllByCategoryId(category.Id.ToString());
		}

		return categories;
	}

	public async Task<Category> Insert(Category category)
	{
		await Task.Delay(1);
		return new Category();
	}

	public async Task<bool> Update(Category category)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(int categoryId)
	{
		await Task.Delay(1);
		return true;
	}
}
