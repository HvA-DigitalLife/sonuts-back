using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface ICarePlanDao
{
	Task<List<CarePlan>> SelectAllByParticipantId ( Guid participantId );
	Task<CarePlan> Insert ( CarePlan carePlan );
	Task<bool> Update ( CarePlan carePlan );
	Task<bool> Delete ( Guid id );
}
