namespace Sonuts.Domain.Entities;

public class Recipe : BaseEntity
{
	public required string Name { get; set; }
	public required Image Image { get; set; }
	public Theme Theme { get; set; } = default!;
	public List<RecipeIngredient> Ingredients { get; set; } = new();
	public List<RecipeStep> Steps { get; set; } = new();
}
