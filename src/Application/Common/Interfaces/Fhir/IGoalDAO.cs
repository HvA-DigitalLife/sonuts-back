using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IGoalDao
{
	Task<Goal> Select ( Guid id);
	Task<Goal> Insert ( Goal goal );
	Task<Goal> Update ( Goal goal );
	Task<bool> Delete ( Guid id );
}