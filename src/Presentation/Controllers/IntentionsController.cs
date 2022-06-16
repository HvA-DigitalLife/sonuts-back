using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;

namespace Sonuts.Presentation.Controllers;

public class IntentionsController : ApiControllerBase
{
	/// <summary>
	/// Get agenda tasks
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<ICollection<IntentionDto>>> GetIntentions()
	{
		await Task.Delay(1);
		return Ok();
	}

	/// <summary>
	/// Plan tasks for agenda
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<IntentionDto>> CreateIntentions()
	{
		await Task.Delay(2);
		return Ok();
	}

	/// <summary>
	/// Complete a task from agenda
	/// </summary>
	/// <returns></returns>
	[HttpPost("{intentionId}")]
	public async Task<ActionResult<ExecutionDto>> CreateExecution()
	{
		await Task.Delay(3);
		return Ok();
	}
}
