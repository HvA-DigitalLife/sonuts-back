using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Categories;

public class CategoryDto : IMapFrom<Category>
{
	public Guid Id { get; set; }
	public bool IsActive { get; set; } = false;
	public string Name { get; set; } = default!;
	public string Color { get; set; } = default!;
	public QuestionnaireDto Questionnaire { get; set; } = default!;
	public ICollection<ThemeDto> Themes { get; set; } = new List<ThemeDto>();
}
