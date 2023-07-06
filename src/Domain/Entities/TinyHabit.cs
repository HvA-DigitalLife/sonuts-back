using System;
namespace Sonuts.Domain.Entities
{
	public class TinyHabit : BaseEntity
	{
		public DateOnly? CreatedAt { get; init; } = DateOnly.FromDateTime(DateTime.Now);
		public required Participant? Participant { get; set; }
		public required Category? Category { get; set; }
		public required string? TinyHabitText { get; set; }
	}
}

