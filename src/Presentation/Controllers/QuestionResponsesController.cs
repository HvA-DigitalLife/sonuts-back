using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Logic.QuestionResponses.Queries;

namespace Sonuts.Presentation.Controllers;

public class QuestionResponsesController : ApiControllerBase
{
	[Authorize(Roles = "Admin")]
	[HttpGet("Csv")]
	public async Task<ActionResult> ExportQuestionResponses(CancellationToken cancellationToken)
	{
		var file = await Mediator.Send(new ExportQuestionResponsesQuery(), cancellationToken);

		return File(file.Content, file.ContentType, file.FileName);
	}
}
