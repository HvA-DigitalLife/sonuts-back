using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Activities.Queries;

namespace Sonuts.Presentation.Controllers;

public class ActivitiesController : ApiControllerBase
{
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet("{activityId:guid}")]
	public async Task<ActionResult<ActivityDto>> GetActivity(Guid activityId)
	{
		return Ok(await Mediator.Send(new GetActivityQuery { Id = activityId }));
	}
}
