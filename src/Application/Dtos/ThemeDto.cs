using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class ThemeDto : IMapFrom<Theme>
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required ThemeType Type { get; set; }
	public required string Description { get; set; }
	public required CategoryDto Category { get; set; }
	public required ImageDto Image { get; set; }
	public required ThemeUnit Unit { get; set; }
	public FrequencyType FrequencyType { get; set; }
	public int? FrequencyGoal { get; set; }
	public required string CurrentFrequencyQuestion { get; set; }
	public required string GoalFrequencyQuestion { get; set; }
	public required string? CurrentActivityQuestion { get; set; }
	public required string? GoalActivityQuestion { get; set; }
	public IList<RecommendationRuleDto> RecommendationRules { get; set; } = Array.Empty<RecommendationRuleDto>();
	public IList<ActivityDto> Activities { get; set; } = Array.Empty<ActivityDto>();
}
