using Sonuts.Application.Dtos;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Themes.Models;

public class RecommendedThemeVm
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public ImageDto Image { get; set; } = default!;
	public FrequencyType FrequencyType { get; set; }
	public int? FrequencyGoal { get; set; }
	public string CurrentQuestion { get; set; } = default!;
	public string GoalQuestion { get; set; } = default!;
	public bool IsRecommended { get; set; }
}
