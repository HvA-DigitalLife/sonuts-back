namespace Sonuts.Domain.Entities;

public class Intention : BaseEntity
{
	public Activity Activity { get; set; } = default!;
	public int FrequencyAmount { get; set; } = default!;
	public Moment Moment { get; set; } = default!;
	public TimeOnly? Reminder { get; set; }
	public Participant Participant { get; set; } = default!;
	public ICollection<Execution> Executions { get; set; } = new List<Execution>();
}

public class Moment : IOwned
{
	public bool OnMonday { get; set; } = default!;
	public bool OnTuesday { get; set; } = default!;
	public bool OnWednesday { get; set; } = default!;
	public bool OnThursday { get; set; } = default!;
	public bool OnFriday { get; set; } = default!;
	public bool OnSaturday { get; set; } = default!;
	public bool OnSunday { get; set; } = default!;
	public TimeOnly Time { get; set; } = default!;
	public MomentType Type { get; set; } = default!;
	public string? EventName { get; set; }
}
