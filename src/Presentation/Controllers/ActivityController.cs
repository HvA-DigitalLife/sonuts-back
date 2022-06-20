using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Activities.Queries;
using Sonuts.Application.Dtos;

namespace Sonuts.Presentation.Controllers;

public class ActivityController : ApiControllerBase
{
	[AllowAnonymous] //TODO: [Authorize(Roles = "Admin, Participant")]
	[HttpGet("{activityId:guid}")]
	public async Task<ActionResult<ActivityDto>> GetActivity(Guid activityId)
	{
		return Ok(await Mediator.Send(new GetActivityQuery { Id = activityId }));
	}
}
