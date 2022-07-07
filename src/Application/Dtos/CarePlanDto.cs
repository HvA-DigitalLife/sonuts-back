using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class CarePlanDto : IMapFrom<CarePlan>
{
	public DateOnly Start { get; set; }
	public DateOnly End { get; set; }
	public ParticipantDto Participant { get; set; } = default!;
	public ICollection<GoalDto> Goals { get; set; } = new List<GoalDto>();
}
