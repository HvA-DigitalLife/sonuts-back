using Sonuts.Application.Logic.Participants.Queries;

namespace Sonuts.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
	Task<byte[]> BuildParticipantsFile(IEnumerable<OverviewParticipantDto> participants);
}
