using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class MomentDto : IMapFrom<Moment>
{
	public DateTime Time { get; set; } = default!;
	public MomentType Type { get; set; } = default!;
	public string? EventName { get; set; }
}
