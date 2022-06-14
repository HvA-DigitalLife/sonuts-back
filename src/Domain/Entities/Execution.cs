namespace Sonuts.Domain.Entities;

public class Execution : BaseEntity
{
	public bool IsDone { get; set; } = default!;
	public DateTime CreatedAt { get; } = DateTime.Now;
	public Intention Intention { get; set; } = default!;
}
