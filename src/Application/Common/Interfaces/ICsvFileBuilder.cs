using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
	Task<byte[]> BuildDynamicFileAsync(List<string> headers, List<List<string?>> rows);

	Task<byte[]> BuildParticipantsFileAsync(IEnumerable<Participant> participants, CancellationToken cancellationToken = default);

	Task<byte[]> BuildExecutionsFileAsync(IEnumerable<Execution> executions, CancellationToken cancellationToken = default);
}
