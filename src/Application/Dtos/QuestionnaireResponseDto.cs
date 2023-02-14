using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class QuestionnaireResponseDto : IMapFrom<QuestionnaireResponse>
{
	public Guid Id { get; set; }
	public DateOnly CreatedAt { get; set; }
	public required QuestionnaireDto Questionnaire { get; set; }
	public required ParticipantDto Participant { get; set; }
	public ICollection<QuestionResponseDto> Responses { get; set; } = new List<QuestionResponseDto>();
}
