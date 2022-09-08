using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Questionnaires.Models;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.QuestionnaireResponses.Models;

public class QuestionnaireResponseVm : IMapFrom<QuestionnaireResponse>
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public QuestionnaireIdVm Questionnaire { get; set; } = default!;
}
