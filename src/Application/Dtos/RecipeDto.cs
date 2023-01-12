using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class RecipeDto : IMapFrom<Recipe>
{
	public Guid Id { get; set; }
	public required string Name { get; set; }

	public required ImageDto Image { get; set; }
	public List<RecipeIngredientDto> Ingredients { get; set; } = new();
	public List<RecipeStepDto> Steps { get; set; } = new();
}
