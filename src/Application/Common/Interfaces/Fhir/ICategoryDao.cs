using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface ICategoryDao
{
	Task<Category> Select ( Guid id );
	Task<List<Category>> SelectAll ();
	Task<List<Category>> Initialize ( List<Category> categories );

}
