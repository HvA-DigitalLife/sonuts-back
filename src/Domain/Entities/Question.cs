using Microsoft.EntityFrameworkCore;

namespace Sonuts.Domain.Entities;

public class Question : BaseEntity
{
	public QuestionType Type { get; set; } = default!;
	public string Text { get; set; } = default!;
	public string? Description { get; set; }
	public int Order { get; set; } = default!;
	public EnableWhen? EnableWhen { get; set; }
	public List<AnswerOption>? AnswerOptions { get; set; } = new();
	public List<RecommendationRule> RecommendationRules { get; set; } = new();
}

[Owned]
public class EnableWhen
{
	public Guid QuestionId { get; set; } = default!;
	public Operator Operator { get; set; } = default!;
	public string Answer { get; set; } = default!;
}
