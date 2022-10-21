using System.Text;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Adapters;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirThemeDao : IThemeDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirThemeDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;



	public async Task<List<Theme>> SelectAllByCategoryId(Guid categoryId)
	{

		// load and parse domains instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("PlanDefinition?type=" + categoryId.ToString()); // todo, add extension and search parameter for mib domain id valueset to FHIR, will get all of them for now

		return FhirThemeAdapter.FromJsonBundle(result);
	}

	public async Task<Theme> Insert(Theme theme)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PutAsync("PlanDefinition/" + theme.Id.ToString(), new StringContent(FhirThemeAdapter.ToJson(theme), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirThemeAdapter.FromJson(responseContent);
	}

	public async Task<bool> Update(Theme theme)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(Guid themeId)
	{
		await Task.Delay(1);
		return true;
	}
}
