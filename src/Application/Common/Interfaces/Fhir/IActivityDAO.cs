using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IActivityDao
{
	Task<Activity> Select (System.Guid id);
	Task<Activity> Insert ( Activity activity );
	Task<Activity> Update ( Activity activity );
	Task<bool> Delete ( System.Guid id );
}