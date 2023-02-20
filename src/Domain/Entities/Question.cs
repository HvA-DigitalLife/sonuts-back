using Microsoft.EntityFrameworkCore;

namespace Sonuts.Domain.Entities;

public class Question : BaseEntity
{
	public required QuestionType Type { get; set; }
	public required string Text { get; set; }
	public string? Description { get; set; }
	public required int Order { get; set; }
	public EnableWhen? EnableWhen { get; set; }
	public List<AnswerOption>? AnswerOptions { get; set; } = new();
	public string? OpenAnswerLabel { get; set; }
	public List<RecommendationRule> RecommendationRules { get; set; } = new();
	public bool IsRequired { get; set; }
	public int? Min { get; set; }
	public int? Max { get; set; }
}

[Owned]
public class EnableWhen
{
	public required Guid DependentQuestionId { get; set; }
	public required Operator Operator { get; set; }
	public required string Answer { get; set; }
}
