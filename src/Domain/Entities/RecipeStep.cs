namespace Sonuts.Domain.Entities;

public class RecipeStep : BaseEntity
{
	public string Description { get; set; } = default!;
	public int Order { get; set; }
}
