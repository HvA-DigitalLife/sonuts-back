namespace Sonuts.Domain.Entities;

public class Faq : BaseEntity
{
	public required string Question { get; set; }
	public required string Answer { get; set; }
	public Theme Theme { get; set; } = default!;
}
