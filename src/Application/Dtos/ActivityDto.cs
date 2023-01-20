using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;


public class ActivityDto : IMapFrom<Activity>
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public required ImageDto Image { get; set; }
	public required ThemeDto Theme { get; set; }
	public List<VideoDto> Videos { get; set; } = new();
}
