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
	public async Task<ActionResult<ThemeDto>> Get(Guid themeId)
	{
		return Ok(await Mediator.Send(new GetThemeQuery
		{
			Id = themeId
		}));
	}

	[Authorize(Roles = "Participant")]
	[HttpGet("{themeId:guid}/Faq")]
	public async Task<ActionResult<ThemeDto>> GetFaq(Guid themeId)
	{
		return Ok(await Mediator.Send(new GetFaqQuery
		{
			ThemeId = themeId
		}));
	}


	[Authorize(Roles = "Participant")]
	[HttpGet("{themeId:guid}/Recipes")]
	public async Task<ActionResult<ThemeDto>> GetRecipes(Guid themeId)
	{
		return Ok(await Mediator.Send(new GetRecipesQuery
		{
			ThemeId = themeId
		}));
	}
}
