using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Executions.Commands;
using Sonuts.Application.Logic.Executions.Queries;

namespace Sonuts.Presentation.Controllers;

public class ExecutionsController : ApiControllerBase
{
	[Authorize(Roles = "Admin")]
	[HttpGet("Csv")]
	public async Task<ActionResult> ExportExecutions(CancellationToken cancellationToken)
	{
		var file = await Mediator.Send(new ExportExecutionsQuery(), cancellationToken);

		return File(file.Content, file.ContentType, file.FileName);
	}

	/// <summary>
	/// Complete a task from agenda
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPut]
	public async Task<ActionResult<ExecutionDto>> CreateExecution(CreateOrUpdateExecutionCommand command, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(command, cancellationToken));
	}
}
