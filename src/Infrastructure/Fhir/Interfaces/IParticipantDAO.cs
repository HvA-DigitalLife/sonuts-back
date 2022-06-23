using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Interfaces;

public interface IParticipantDao
{
	Task<Participant> Insert ( Participant participant );
	Task<bool> Update ( Participant participant );
	Task<bool> Delete ( int participantId );
}
