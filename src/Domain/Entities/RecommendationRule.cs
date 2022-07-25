namespace Sonuts.Domain.Entities;

public class RecommendationRule : BaseEntity
{
	public RecommendationRuleType Type { get; set; }
	public Operator Operator { get; set; }
	public string Value { get; set; } = default!;
	public List<Question> Questions { get; set; } = new();
}
