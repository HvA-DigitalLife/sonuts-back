namespace Sonuts.Domain.Entities;

public class Theme : BaseEntity //http://hl7.org/fhir/R4/plandefinition.html#PlanDefinition
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public Category Category { get; set; } = default!;
	public Image Image { get; set; } = default!;
	public FrequencyType FrequencyType { get; set; } = default!;
	public int? FrequencyGoal { get; set; } = default!;
	public string CurrentFrequencyQuestion { get; set; } = default!;
	public string GoalFrequencyQuestion { get; set; } = default!;
	public string? CurrentActivityQuestion { get; set; } = default!;
	public string? GoalActivityQuestion { get; set; } = default!;
	public List<RecommendationRule> RecommendationRules { get; set; } = new();
	public List<Activity> Activities { get; set; } = new();
}
