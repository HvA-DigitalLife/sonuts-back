using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Logic.Questionnaires.Models;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.QuestionnaireResponses.Models;

public class QuestionnaireResponseVm : IMapFrom<QuestionnaireResponse>
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public required QuestionnaireIdVm Questionnaire { get; set; }
}
