using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IParticipantDao
{
	Task<Participant> Insert ( Participant participant );
	Task<bool> Update ( Participant participant );
	Task<bool> Delete ( Guid id );
}
