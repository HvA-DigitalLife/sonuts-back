namespace Sonuts.Domain.Entities.Owned;

public class QuestionDependency : IOwned
{
	public Guid QuestionId { get; set; } = default!;
	public Operator Operator { get; set; } = default!;
	public string Value { get; set; } = default!;
}
