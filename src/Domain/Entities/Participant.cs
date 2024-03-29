namespace Sonuts.Domain.Entities;

public class Participant : BaseEntity //TODO
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public DateOnly? Birth { get; set; }
	public string? Gender { get; set; }
	public decimal? Weight { get; set; }
	public decimal? Height { get; set; }
	public string? MaritalStatus { get; set; }
	public bool IsActive { get; set; } = true;
	public ICollection<CarePlan> CarePlans { get; set; } = new List<CarePlan>();
}
