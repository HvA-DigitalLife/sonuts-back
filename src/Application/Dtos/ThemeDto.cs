using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class ThemeDto : IMapFrom<Theme>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public CategoryDto Category { get; set; } = default!;
	public ImageDto Image { get; set; } = default!;
	public FrequencyType FrequencyType { get; set; }
	public int? FrequencyGoal { get; set; }
	public string CurrentFrequencyQuestion { get; set; } = default!;
	public string GoalFrequencyQuestion { get; set; } = default!;
	public string? CurrentActivityQuestion { get; set; } = default!;
	public string? GoalActivityQuestion { get; set; } = default!;
	public List<RecommendationRuleDto> RecommendationRules { get; set; } = new();
	public List<ActivityDto> Activities { get; set; } = new();
}
