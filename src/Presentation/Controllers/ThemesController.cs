using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Faq.Queries;
using Sonuts.Application.Logic.Recipes.Queries;
using Sonuts.Application.Logic.Themes.Queries;

namespace Sonuts.Presentation.Controllers;

public class ThemesController : ApiControllerBase
{
	[Authorize(Roles = "Participant")]
	[HttpGet("{themeId:guid}")]
	public async Task<ActionResult<ThemeDto>> Get(Guid themeId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetThemeQuery
		{
			Id = themeId
		}, cancellationToken));
	}

	[Authorize(Roles = "Participant")]
	[HttpGet("{themeId:guid}/Faq")]
	public async Task<ActionResult<List<FaqDto>>> GetFaq(Guid themeId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetFaqQuery
		{
			ThemeId = themeId
		}, cancellationToken));
	}


	[Authorize(Roles = "Participant")]
	[HttpGet("{themeId:guid}/Recipes")]
	public async Task<ActionResult<List<RecipeDto>>> GetRecipes(Guid themeId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetRecipesQuery
		{
			ThemeId = themeId
		}, cancellationToken));
	}
}
