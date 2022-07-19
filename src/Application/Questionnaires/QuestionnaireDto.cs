using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Questionnaires;

public class QuestionnaireDto : IMapFrom<Questionnaire>
{
	public Guid Id { get; set; }
	public string Title { get; set; } = default!;
	public string? Description { get; set; }
	public ICollection<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
}