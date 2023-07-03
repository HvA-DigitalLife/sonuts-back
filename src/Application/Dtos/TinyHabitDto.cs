using System;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos
{
	public class TinyHabitDto : Common.Mappings.IMapFrom<TinyHabit>
    {
		public Guid Id { get; set; }
		public DateOnly CreatedAt { get; set; }
		public required Participant Participant { get; set; }
		public required Category Category { get; set; }
		public required string TinyHabitText { get; set; }
	}
}

