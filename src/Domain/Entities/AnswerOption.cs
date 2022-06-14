namespace Sonuts.Domain.Entities;

public class AnswerOption : BaseEntity
{
	public string Text { get; set; } = default!;
	public int Order { get; set; } = default!;
}
