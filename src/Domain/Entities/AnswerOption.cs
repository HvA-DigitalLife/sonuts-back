namespace Sonuts.Domain.Entities;

public class AnswerOption : BaseEntity
{
	public string Value { get; set; } = default!;
	public int Order { get; set; } = default!;
}
