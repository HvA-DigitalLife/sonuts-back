using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Themes.Queries;

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
}
