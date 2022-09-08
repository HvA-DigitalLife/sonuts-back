using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ParticipantDto : IMapFrom<Participant>
{
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateOnly? Birth { get; set; }
	public string? Gender { get; set; }
	public decimal? Weight { get; set; }
	public decimal? Height { get; set; }
	public string? MaritalStatus { get; set; }
	public bool IsActive { get; set; } = true;
}
