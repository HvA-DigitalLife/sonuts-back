namespace Sonuts.Domain.Entities;

public class RecommendationRule : BaseEntity
{
	public RecommendationRuleType Type { get; set; }
	public List<Question> Questions { get; set; } = new();
}
