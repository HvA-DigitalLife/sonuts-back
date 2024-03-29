using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Recipes.Queries;

namespace Sonuts.Presentation.Controllers;

public class RecipesController : ApiControllerBase
{
	[Authorize(Roles = "Participant")]
	[HttpGet("{recipeId:guid}")]
	public async Task<ActionResult<RecipeDto>> GetRecipe(Guid recipeId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetRecipeQuery
		{
			Id = recipeId
		}, cancellationToken));
	}
}
