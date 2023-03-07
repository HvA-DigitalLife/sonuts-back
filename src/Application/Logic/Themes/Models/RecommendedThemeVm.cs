using Sonuts.Application.Dtos;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Themes.Models;

public record RecommendedThemeVm
{
	public Guid Id { get; init; }
	public required string Name { get; init; }
	public required string Description { get; init; }
	public required ThemeType Type { get; init; }
	public required ImageDto Image { get; init; }
	public required ThemeUnit Unit { get; init; }
	public int? UnitAmount { get; init; }
	public FrequencyType FrequencyType { get; init; }
	public int? FrequencyGoal { get; init; }
	public required string? CurrentFrequencyQuestion { get; init; }
	public required string? GoalFrequencyQuestion { get; init; }
	public required string? CurrentActivityQuestion { get; init; }
	public required string? GoalActivityQuestion { get; init; }
	public bool IsRecommended { get; init; }
}
