using Sonuts.Application.Logic.Themes.Models;

namespace Sonuts.Application.Logic.Categories.Models;

public class CategoriesWithRecommendationsVm
{
	public Guid Id { get; set; }
	public bool IsActive { get; set; } = false;
	public required string Name { get; set; }
	public required string Color { get; set; }
	public List<RecommendedThemeVm> Themes { get; set; } = new();
}
