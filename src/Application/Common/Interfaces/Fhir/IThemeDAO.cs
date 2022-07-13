using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IThemeDao
{
	Task<List<Theme>> SelectAllByCategoryId (string categoryId);
	Task<Theme> Insert ( Theme guideline );
	Task<bool> Update ( Theme guideline );
	Task<bool> Delete ( int themeId );
}
