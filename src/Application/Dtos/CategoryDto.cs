using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

/// <summary>
/// Category: ie exercise, nutrition
/// </summary>
public class CategoryDto : IMapFrom<Category>
{
	public Guid Id { get; set; }
	public bool IsActive { get; set; } = false;
	/// <summary>
	/// Category name
	/// </summary>
	public string Name { get; set; } = default!;
	/// <summary>
	/// User interface color
	/// </summary>
	public string Color { get; set; } = default!;
	/// <summary>
	/// Reference to questionnaire linked to this Category
	/// </summary>
	/// <value></value>
	public QuestionnaireDto Questionnaire { get; set; } = default!;
	/// <summary>
	/// List of themes related to this category: ie biking, walking.
	/// </summary>
	/// <returns></returns>
	public List<ThemeDto> Themes { get; set; } = new();
}
