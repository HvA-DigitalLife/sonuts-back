using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface ICarePlanDao
{
	Task<List<CarePlan>> SelectAllByParticipantId (string participantId);
	Task<CarePlan> Insert ( CarePlan carePlan );
	Task<bool> Update ( CarePlan carePlan );
	Task<bool> Delete ( int carePlanId );
}
