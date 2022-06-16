using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ExecutionDto : IMapFrom<Execution>
{
	public bool IsDone { get; set; } = default!;
	public DateTime CreatedAt { get; set; } = default!;
	public Intention Intention { get; set; } = default!;
}
