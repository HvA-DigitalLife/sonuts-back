using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Goals.Commands;
using Sonuts.Application.Logic.Goals.Queries;

namespace Sonuts.Presentation.Controllers;

public class GoalsController : ApiControllerBase
{
	/// <summary>
	/// Get agenda tasks
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpGet]
	public async Task<ActionResult<ICollection<GoalDto>>> GetGoals(CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetGoalsQuery(), cancellationToken));
	}

	/// <summary>
	/// Change goal moment 
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPatch("{id:guid}")]
	public async Task<ActionResult<ExecutionDto>> CreateExecution(Guid id, ChangeGoalMomentCommand command, CancellationToken cancellationToken)
	{
		if (!id.Equals(command.Id))
			return BadRequest();

		return Ok(await Mediator.Send(command, cancellationToken));
	}
}
