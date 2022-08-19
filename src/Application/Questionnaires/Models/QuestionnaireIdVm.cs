using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Questionnaires.Models;
public class QuestionnaireIdVm : IMapFrom<Questionnaire>
{
	public Guid Id { get; set; }
}
