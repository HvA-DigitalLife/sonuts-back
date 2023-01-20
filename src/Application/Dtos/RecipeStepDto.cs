using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class RecipeStepDto : IMapFrom<RecipeStep>
{
	public required string Description { get; set; }
	public int Order { get; set; }
}
