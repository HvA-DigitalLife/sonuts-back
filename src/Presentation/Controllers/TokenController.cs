using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Token.Commands;
using Sonuts.Application.Token.Models;

namespace Sonuts.Presentation.Controllers;

public class TokenController : ApiControllerBase
{
	[AllowAnonymous]
	[HttpPost]
	public async Task<ActionResult<TokenVm>> CreateToken(CreateTokenCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}