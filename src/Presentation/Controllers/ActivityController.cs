using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;

namespace Sonuts.Presentation.Controllers;

public class ActivityController : ApiControllerBase
{
	[HttpGet("{activityId:guid}")]
	public async Task<ActionResult<ActivityDto>> GetActivity([FromBody] Guid activityId)
	{
		await Task.Delay(1);
		return Ok();
	}
}
