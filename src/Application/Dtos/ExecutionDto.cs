using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ExecutionDto : IMapFrom<Execution>
{
	public Guid Id { get; set; }
	public bool IsDone { get; set; } = default!;
	public DateTime CreatedAt { get; set; } = default!;
}
