using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class CategoryDto : IMapFrom<Category>
{
	public Guid Id { get; set; }
	public bool IsActive { get; set; } = false;
	public string Name { get; set; } = default!;
	public string Color { get; set; } = default!;
	public QuestionnaireDto Questionnaire { get; set; } = default!;
	public List<ThemeDto> Themes { get; set; } = new();
}
