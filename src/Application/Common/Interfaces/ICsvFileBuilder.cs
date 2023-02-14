using Sonuts.Application.Logic.Participants.Queries;

namespace Sonuts.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
	Task<byte[]> BuildDynamicFile(List<string> headers, List<List<string?>> rows);
	Task<byte[]> BuildParticipantsFile(IEnumerable<OverviewParticipantDto> participants);
}
