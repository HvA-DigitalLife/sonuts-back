namespace Sonuts.Domain.Entities;

public class Execution : BaseEntity
{
	public bool IsDone { get; set; } = default!;
	public DateTime CreatedAt { get; init; } = DateTime.Now;
	public Goal Goal { get; set; } = default!;
}
