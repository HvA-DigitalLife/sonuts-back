namespace Sonuts.Domain.Entities;

public class Theme : BaseEntity //http://hl7.org/fhir/R4/plandefinition.html#PlanDefinition
{
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required ThemeType Type { get; set; }
	public Category Category { get; set; } = default!;
	public required Image Image { get; set; }
	public required ThemeUnit Unit { get; set; }
	public required FrequencyType FrequencyType { get; set; }
	public int? FrequencyGoal { get; set; }
	public required string CurrentFrequencyQuestion { get; set; }
	public required string GoalFrequencyQuestion { get; set; }
	public string? CurrentActivityQuestion { get; set; }
	public string? GoalActivityQuestion { get; set; }
	public ICollection<RecommendationRule> RecommendationRules { get; set; } = new List<RecommendationRule>();
	public ICollection<Activity> Activities { get; set; } = new List<Activity>();
	public ICollection<Faq> Faq { get; set; } = new List<Faq>();
	public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
