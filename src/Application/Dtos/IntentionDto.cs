using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class IntentionDto : IMapFrom<Goal>
{
	public Guid Id { get; set; }
	public ActivityDto Activity { get; set; } = default!;
	public int FrequencyAmount { get; set; } = default!;
	public MomentDto Moment { get; set; } = default!;
	public TimeOnly? Reminder { get; set; }
	public ParticipantDto Participant { get; set; } = default!;
	public ICollection<ExecutionDto> Executions { get; set; } = new List<ExecutionDto>();
}
