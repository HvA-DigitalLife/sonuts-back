using Sonuts.Application.Common.Mappings;
using Sonuts.Application.QuestionnaireResponses;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class QuestionResponseDto : IMapFrom<QuestionResponse>
{
	public QuestionnaireResponseDto QuestionnaireResponse { get; set; } = default!;
	public QuestionDto Question { get; set; } = default!;
	public string Answer { get; set; } = default!;
}
