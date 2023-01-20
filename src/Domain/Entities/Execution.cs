namespace Sonuts.Domain.Entities;

public class Execution : BaseEntity
{
	public required bool IsDone { get; set; }
	public required int Amount { get; set; }
	public string? Reason { get; set; }
	public DateTime CreatedAt { get; init; } = DateTime.Now;
	public required Goal Goal { get; set; }
}
