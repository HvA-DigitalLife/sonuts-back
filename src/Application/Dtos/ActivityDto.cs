using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ActivityDto : IMapFrom<Activity>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
	public ImageDto Image { get; set; } = default!;
	public ThemeDto Theme { get; set; } = default!;
	public List<VideoDto> Video { get; set; } = new();
}
