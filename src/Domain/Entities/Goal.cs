using Microsoft.EntityFrameworkCore;

namespace Sonuts.Domain.Entities;

public class Goal : BaseEntity
{
	public string? CustomName { get; set; }
	public required Activity Activity { get; set; }
	public required int FrequencyAmount { get; set; }
	public required Moment Moment { get; set; }
	public TimeOnly? Reminder { get; set; }
	public CarePlan CarePlan { get; set; } = default!;
	public ICollection<Execution> Executions { get; set; } = new List<Execution>();
}

[Owned]
public class Moment
{
	public required DayOfWeek Day { get; set; }
	public TimeOnly? Time { get; set; }
	public required MomentType Type { get; set; }
	public string? EventName { get; set; }
}
