using Microsoft.EntityFrameworkCore;

namespace Sonuts.Domain.Entities.Owned;

[Owned]
public class QuestionDependency
{
	public Guid QuestionId { get; set; } = default!;
	public Operator Operator { get; set; } = default!;
	public string Value { get; set; } = default!;
}
