using Sonuts.Application.Oauth2.Commands;
using Sonuts.Application.Oauth2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sonuts.Presentation.Controllers;

[Authorize]
public class Oauth2Controller : ApiControllerBase
{
	[AllowAnonymous]
	[HttpPost("/oauth2/token")]
	public async Task<ActionResult<TokenVm>> CreateToken(CreateTokenCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
