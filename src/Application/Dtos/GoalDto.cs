using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class GoalDto : IMapFrom<Goal>
{
	public Guid Id { get; set; }
	public ActivityDto Activity { get; set; } = default!;
	public int FrequencyAmount { get; set; } = default!;
	public MomentDto Moment { get; set; } = default!;
	public TimeOnly? Reminder { get; set; }
	public ICollection<ExecutionDto> Executions { get; set; } = new List<ExecutionDto>();
}
