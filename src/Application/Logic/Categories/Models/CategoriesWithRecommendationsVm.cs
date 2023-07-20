using Sonuts.Application.Logic.Themes.Models;

namespace Sonuts.Application.Logic.Categories.Models;

public record CategoriesWithRecommendationsVm
{
	public Guid Id { get; init; }
	public bool IsActive { get; init; } = false;
	public required string Name { get; init; }
	public required string Color { get; init; }
	public IList<RecommendedThemeVm> Themes { get; init; } = Array.Empty<RecommendedThemeVm>();
}
