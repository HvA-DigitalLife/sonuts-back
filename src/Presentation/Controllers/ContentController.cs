using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Content;
using Sonuts.Application.Content.Commands;
using Sonuts.Application.Content.Queries;
using Sonuts.Domain.Enums;

namespace Sonuts.Presentation.Controllers;

public class ContentController : ApiControllerBase
{
	/// <summary>
	/// Get text for screens
	/// </summary>
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet("{type}")]
	public async Task<ActionResult<ContentDto>> GetContentByType(ContentType? type)
	{
		return Ok(await Mediator.Send(new GetContentByTypeQuery { Type = type }));
	}

	[ApiExplorerSettings(IgnoreApi = true)]
	[Authorize(Roles = "Admin")]
	[HttpPatch("{type}")]
	public async Task<ActionResult<ContentDto>> UpdateContent(ContentType type, UpdateContentCommand command)
	{
		if (!type.Equals(command.Type)) BadRequest();

		return Ok(await Mediator.Send(command));
	}
}
