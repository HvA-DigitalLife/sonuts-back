using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities.Owned;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class MomentDto : IMapFrom<Moment>
{
	public bool OnMonday { get; set; } = default!;
	public bool OnTuesday { get; set; } = default!;
	public bool OnWednesday { get; set; } = default!;
	public bool OnThursday { get; set; } = default!;
	public bool OnFriday { get; set; } = default!;
	public bool OnSaturday { get; set; } = default!;
	public bool OnSunday { get; set; } = default!;
	public TimeOnly Time { get; set; } = default!;
	public MomentType Type { get; set; } = default!;
	public string? EventName { get; set; }
}
