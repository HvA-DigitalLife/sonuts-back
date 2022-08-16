using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Questionnaires;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class QuestionnaireResponseDto : IMapFrom<QuestionnaireResponse>
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public QuestionnaireDto Questionnaire { get; set; } = default!;
	public ParticipantDto Participant { get; set; } = default!;
	public ICollection<QuestionResponseDto> Responses { get; set; } = new List<QuestionResponseDto>();
}
