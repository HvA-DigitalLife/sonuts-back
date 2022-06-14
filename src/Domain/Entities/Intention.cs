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
