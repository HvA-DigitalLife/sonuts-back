using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ParticipantDto : IMapFrom<Participant>
{
	public DateOnly? Birth { get; set; }
	public string? Gender { get; set; }
	public decimal? Weight { get; set; }
	public decimal? Height { get; set; }
	public string? MaritalStatus { get; set; }
	public bool IsActive { get; set; } = true;
	public ICollection<IntentionDto> Intentions { get; set; } = new List<IntentionDto>();
}
