using System;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class TinyHabitDto : Common.Mappings.IMapFrom<TinyHabit>
    {
	public Guid? Id { get; set; }
	public DateOnly? CreatedAt { get; set; }
	public required Guid? ParticipantId { get; set; }
	public required string? TinyHabitText { get; set; }
}

