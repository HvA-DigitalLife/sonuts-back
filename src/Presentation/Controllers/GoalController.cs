using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Executions.Commands;
using Sonuts.Application.Goals.Commands;
using Sonuts.Application.Goals.Queries;

namespace Sonuts.Presentation.Controllers;

public class GoalController : ApiControllerBase
{
	/// <summary>
	/// Get agenda tasks
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpGet]
	public async Task<ActionResult<ICollection<GoalDto>>> GetGoals()
	{
		return Ok(await Mediator.Send(new GetGoalsQuery()));
	}

	/// <summary>
	/// Plan tasks for agenda
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPost]
	public async Task<ActionResult<GoalDto>> CreateGoals(CreateGoalsCommand command)
	{
		return Ok(await Mediator.Send(command));
	}

	/// <summary>
	/// Complete a task from agenda
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Participant")]
	[HttpPost("{goalId:guid}")]
	public async Task<ActionResult<ExecutionDto>> CreateExecution(Guid goalId, CreateExecutionCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
