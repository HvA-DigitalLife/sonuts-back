using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class RecipeIngredientDto : IMapFrom<RecipeIngredient>
{
	public required string Ingredient { get; set; }
}
