namespace Sonuts.Domain.Entities;

public class Participant : BaseEntity
{
	public DateOnly? Birth { get; set; }
	public string? Gender { get; set; }
	public decimal? Weight { get; set; }
	public decimal? Height { get; set; }
	public string? MaritalStatus { get; set; }
	public bool IsActive { get; set; } = true;
	public ICollection<Intention> Intentions { get; set; } = new List<Intention>();
}
