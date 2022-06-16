using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.QuestionnaireResponses;
using Sonuts.Application.QuestionnaireResponses.Commands;

namespace Sonuts.Presentation.Controllers;

public class QuestionnaireResponsesController : ApiControllerBase
{
	/// <summary>
	/// Answer a questionnaire
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<QuestionnaireResponseDto>> CreateQuestionnaireResponse(CreateQuestionnaireResponseCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
