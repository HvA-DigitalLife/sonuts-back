using Sonuts.Application.Themes.Models;

namespace Sonuts.Application.Categories.Models;

public class CategoriesWithRecommendationsVm
{
	public Guid Id { get; set; }
	public bool IsActive { get; set; } = false;
	public string Name { get; set; } = default!;
	public string Color { get; set; } = default!;
	public List<RecommendedThemeVm> Themes { get; set; } = new();
}
