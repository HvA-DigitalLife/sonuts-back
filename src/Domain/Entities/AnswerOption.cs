namespace Sonuts.Domain.Entities;

public class AnswerOption : BaseEntity
{
	public required string Name { get; set; }
	public required string Value { get; set; }
	public required int Order { get; set; }
}
