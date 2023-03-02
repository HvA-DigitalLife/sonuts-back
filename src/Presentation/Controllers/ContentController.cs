using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Content.Commands;
using Sonuts.Application.Logic.Content.Queries;
using Sonuts.Domain.Enums;

namespace Sonuts.Presentation.Controllers;

public class ContentController : ApiControllerBase
{
	/// <summary>
	/// Get text for screens
	/// </summary>
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet("{type}")]
	public async Task<ActionResult<ContentDto>> GetContentByType(ContentType? type, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetContentByTypeQuery
		{
			Type = type
		}, cancellationToken));
	}

	[ApiExplorerSettings(IgnoreApi = true)]
	[Authorize(Roles = "Admin")]
	[HttpPatch("{type}")]
	public async Task<ActionResult<ContentDto>> UpdateContent(ContentType type, UpdateContentCommand command, CancellationToken cancellationToken)
	{
		if (!type.Equals(command.Type))
			return BadRequest();

		return Ok(await Mediator.Send(command, cancellationToken));
	}
}
