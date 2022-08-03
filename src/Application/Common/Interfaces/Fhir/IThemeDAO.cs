using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IThemeDao
{
	Task<List<Theme>> SelectAllByCategoryId (string categoryId);
	Task<Theme> Insert ( Theme theme );
	Task<bool> Update ( Theme theme );
	Task<bool> Delete ( int themeId );
}
