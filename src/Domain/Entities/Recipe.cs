namespace Sonuts.Domain.Entities;

public class Recipe : BaseEntity
{
	public string Name { get; set; } = default!;

	public Image Image { get; set; } = default!;
	public Theme Theme { get; set; } = default!;
	public List<RecipeIngredient> Ingredients { get; set; } = new();
	public List<RecipeStep> Steps { get; set; } = new();
}
