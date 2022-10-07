using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class RecipeStepDto : IMapFrom<RecipeStep>
{
	public string Description { get; set; } = default!;
	public int Order { get; set; }
}
