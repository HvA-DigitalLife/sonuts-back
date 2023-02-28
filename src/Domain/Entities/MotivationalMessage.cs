namespace Sonuts.Domain.Entities;

public class MotivationalMessage : BaseEntity
{
	public required string Message { get; set; }

	public required int MinPercentage { get; set; }

	public required int MaxPercentage { get; set; }
}
