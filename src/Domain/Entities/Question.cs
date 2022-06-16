namespace Sonuts.Domain.Entities;

public class Question : BaseEntity
{
	public QuestionType Type { get; set; } = default!;
	public string Text { get; set; } = default!;
	public string? Description { get; set; }
	public int Order { get; set; } = default!;
	public int? MaxAnswers { get; set; }
	public QuestionDependency? QuestionDependency { get; set; }
	public ICollection<AnswerOption>? AnswerOptions { get; set; } = new List<AnswerOption>();
}
