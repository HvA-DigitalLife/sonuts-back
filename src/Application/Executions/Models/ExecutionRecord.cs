using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Executions.Models;

public class ExecutionRecord : IMapFrom<Execution>
{
	public bool IsDone { get; set; } = default!;
	public DateTime CreatedAt { get; set; }
	public Goal Goal { get; set; } = default!;
}
