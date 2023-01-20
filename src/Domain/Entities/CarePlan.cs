namespace Sonuts.Domain.Entities;

public class CarePlan : BaseEntity
{
	public DateOnly Start { get; set; }
	public DateOnly End { get; set; }
	public required Participant Participant { get; set; }
	public List<Goal> Goals { get; set; } = new();
}
