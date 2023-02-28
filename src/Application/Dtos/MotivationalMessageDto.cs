using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public record MotivationalMessageDto : IMapFrom<MotivationalMessage>
{
	public required string Message { get; init; }

	public required int MinPercentage { get; init; }

	public required int MaxPercentage { get; init; }
}
