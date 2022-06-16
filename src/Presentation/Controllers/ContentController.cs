using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Content;
using Sonuts.Application.Content.Commands;
using Sonuts.Application.Content.Queries;
using Sonuts.Domain.Enums;

namespace Sonuts.Presentation.Controllers;

[AllowAnonymous]
public class ContentController : ApiControllerBase
{
	/// <summary>
	/// Get text for screens
	/// </summary>
	[HttpGet("{type}")]
	public async Task<ActionResult<ContentDto>> GetContentByType([FromRoute] ContentType? type)
	{
		return await Mediator.Send(new GetContentByTypeQuery{ Type = type });
	}

	[ApiExplorerSettings(IgnoreApi = true)]
	[Authorize(Roles = "Admin")]
	[HttpPatch("{type}")]
	public async Task<ActionResult<ContentDto>> UpdateContent([FromRoute] ContentType type, [FromBody] UpdateContentCommand command)
	{
		if (!type.Equals(command.Type)) BadRequest();

		return await Mediator.Send(command);
	}
}
