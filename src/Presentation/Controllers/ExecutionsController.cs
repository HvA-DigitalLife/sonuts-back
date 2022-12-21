using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Executions.Commands;

namespace Sonuts.Presentation.Controllers;

public class ExecutionsController : ApiControllerBase
{
	/// <summary>
	/// Complete a task from agenda
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPut]
	public async Task<ActionResult<ExecutionDto>> CreateExecution(CreateOrUpdateExecutionCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
