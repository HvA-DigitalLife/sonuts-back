using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class MomentDto : IMapFrom<Moment>
{
	public required DayOfWeek Day { get; set; }
	public TimeOnly? Time { get; set; }
	public required MomentType Type { get; set; }
	public string? EventName { get; set; }
}
