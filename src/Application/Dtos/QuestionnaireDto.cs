using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class QuestionnaireDto : IMapFrom<Questionnaire>
{
	public Guid Id { get; set; }
	/// <summary>
	/// Test
	/// </summary>
	/// <value></value>
	public required string Title { get; set; }
	public string? Description { get; set; }
	public ICollection<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
}
