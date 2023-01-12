using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class GoalDto : IMapFrom<Goal>
{
	public Guid Id { get; set; }
	public string? CustomName { get; set; }
	public required ActivityDto Activity { get; set; }
	public required int FrequencyAmount { get; set; }
	public required MomentDto Moment { get; set; }
	public TimeOnly? Reminder { get; set; }
	public ICollection<ExecutionDto> Executions { get; set; } = new List<ExecutionDto>();
}
