namespace Sonuts.Domain.Entities;

public class RecipeStep : BaseEntity
{
	public required string Description { get; set; }
	public int Order { get; set; }
}
