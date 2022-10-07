using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class RecipeDto : IMapFrom<Recipe>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;

	public Image Image { get; set; } = default!;
	public List<RecipeIngredientDto> Ingredients { get; set; } = new();
	public List<RecipeStepDto> Steps { get; set; } = new();
}
