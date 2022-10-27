using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IExecutionDao
{
	Task<Execution> Select ( Guid id);
	Task<Execution> Insert ( Execution execution );
	Task<Execution> Update ( Execution execution );
	Task<bool> Delete ( Guid id );
}