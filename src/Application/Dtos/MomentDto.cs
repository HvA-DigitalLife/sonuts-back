using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class MomentDto : IMapFrom<Moment>
{
	public DayOfWeek Day { get; set; } = default!;
	public TimeOnly? Time { get; set; }
	public MomentType Type { get; set; } = default!;
	public string? EventName { get; set; }
}
