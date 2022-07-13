using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface ICategoryDao
{
	Task<List<Category>> SelectAll ();
	Task<Category> Insert ( Category category );
	Task<bool> Update ( Category category );
	Task<bool> Delete ( int categoryId );
}
