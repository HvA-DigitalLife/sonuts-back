using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ExecutionDto : IMapFrom<Execution>
{
	public Guid Id { get; init; }
	public required bool IsDone { get; init; }
	public required int Amount { get; init; }
	public string? Reason { get; init; }
	public required DateOnly CreatedAt { get; init; }
}
