using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class CategoryDto : IMapFrom<Category>
{
	public Guid Id { get; set; }
	public bool IsActive { get; set; } = false;
	public required string Name { get; set; }
	public required string Color { get; set; }
	public required QuestionnaireDto Questionnaire { get; set; }
	public List<ThemeDto> Themes { get; set; } = new();
}
