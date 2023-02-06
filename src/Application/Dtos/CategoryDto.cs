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
	public required string Name { get; set; }
	/// <summary>
	/// User interface color
	/// </summary>
	public required string Color { get; set; }
	/// <summary>
	/// Reference to questionnaire linked to this Category
	/// </summary>
	/// <value></value>
	public required QuestionnaireDto Questionnaire { get; set; }
	/// <summary>
	/// List of themes related to this category: ie biking, walking.
	/// </summary>
	/// <returns></returns>
	public List<ThemeDto> Themes { get; set; } = new();
}
