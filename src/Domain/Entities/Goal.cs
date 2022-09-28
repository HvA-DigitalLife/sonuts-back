using Microsoft.EntityFrameworkCore;

namespace Sonuts.Domain.Entities;

public class Goal : BaseEntity
{
	public string? CustomName { get; set; }
	public Activity Activity { get; set; } = default!;
	public int FrequencyAmount { get; set; } = default!;
	public Moment Moment { get; set; } = default!;
	public TimeOnly? Reminder { get; set; }
	public CarePlan CarePlan { get; set; } = default!;
	public ICollection<Execution> Executions { get; set; } = new List<Execution>();
}

[Owned]
public class Moment
{
	public DayOfWeek Day { get; set; } = default!;
	public TimeOnly? Time { get; set; }
	public MomentType Type { get; set; } = default!;
	public string? EventName { get; set; }
}
