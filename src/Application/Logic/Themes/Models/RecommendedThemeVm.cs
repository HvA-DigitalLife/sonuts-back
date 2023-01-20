using Sonuts.Application.Dtos;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Themes.Models;

public class RecommendedThemeVm
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required ImageDto Image { get; set; }
	public FrequencyType FrequencyType { get; set; }
	public int? FrequencyGoal { get; set; }
	public required string? CurrentFrequencyQuestion { get; set; }
	public required string? GoalFrequencyQuestion { get; set; }
	public required string? CurrentActivityQuestion { get; set; }
	public required string? GoalActivityQuestion { get; set; }
	public bool IsRecommended { get; set; }
}
