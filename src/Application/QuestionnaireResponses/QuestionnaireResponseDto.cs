using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.QuestionnaireResponses;

public class QuestionnaireResponseDto : IMapFrom<QuestionnaireResponse>
{
	public DateTime CreatedAt { get; set; }
	public Questionnaire Questionnaire { get; set; } = default!;
	public Participant Participant { get; set; } = default!;
	public ICollection<QuestionResponseDto> Responses { get; set; } = new List<QuestionResponseDto>();
}
