namespace Sonuts.Domain.Entities;

public class RecommendationRule : BaseEntity
{
	public RecommendationRuleType Type { get; set; }
	public Operator Operator { get; set; }
	public required string Value { get; set; }
	public List<Question> Questions { get; set; } = new();
}
