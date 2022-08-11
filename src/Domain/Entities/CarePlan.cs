namespace Sonuts.Domain.Entities;

public class CarePlan : BaseEntity
{
	public DateOnly Start { get; set; }
	public DateOnly End { get; set; }
	public Participant Participant { get; set; } = default!;
	public List<Goal> Goals { get; set; } = new();
}
