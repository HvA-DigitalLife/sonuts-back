using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Logic.Oauth2.Commands;
using Sonuts.Application.Logic.Oauth2.Models;

namespace Sonuts.Presentation.Controllers;

public class Oauth2Controller : ApiControllerBase
{
	[AllowAnonymous]
	[HttpPost("/oauth2/token")]
	public async Task<ActionResult<TokenVm>> CreateToken(CreateTokenCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
