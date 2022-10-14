using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IGoalDao
{
	Task<Goal> Select (System.Guid id);
	Task<Goal> Insert ( Goal goal );
	Task<Goal> Update ( Goal goal );
	Task<bool> Delete ( System.Guid id );
}