using System;
namespace Sonuts.Domain.Entities
{
	public class TinyHabit : BaseEntity
	{
		public DateOnly? CreatedAt { get; init; } = DateOnly.FromDateTime(DateTime.Now);
		public required Guid? ParticipantId { get; set; }
		public required string? TinyHabitText { get; set; }
	}
}

