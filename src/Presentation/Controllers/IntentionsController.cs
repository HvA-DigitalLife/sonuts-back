using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Executions.Commands;
using Sonuts.Application.Intentions.Commands;
using Sonuts.Application.Intentions.Queries;

namespace Sonuts.Presentation.Controllers;

public class IntentionsController : ApiControllerBase
{
	/// <summary>
	/// Get agenda tasks
	/// </summary>
	[AllowAnonymous] //TODO: [Authorize(Roles = "Participant")]
	[HttpGet]
	public async Task<ActionResult<ICollection<GoalDto>>> GetIntentions()
	{
		return Ok(await Mediator.Send(new GetIntentionsQuery()));
	}

	/// <summary>
	/// Plan tasks for agenda
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPost]
	public async Task<ActionResult<GoalDto>> CreateIntentions(CreateIntentionsCommand command)
	{
		return Ok(await Mediator.Send(command));
	}

	/// <summary>
	/// Complete a task from agenda
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Participant")]
	[HttpPost("{intentionId:guid}")]
	public async Task<ActionResult<ExecutionDto>> CreateExecution(Guid intentionId, CreateExecutionCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
