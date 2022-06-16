using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.QuestionnaireResponses;
using Sonuts.Application.QuestionnaireResponses.Commands;

namespace Sonuts.Presentation.Controllers;

public class QuestionnaireResponsesController : ApiControllerBase
{
	/// <summary>
	/// Answer a questionnaire
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPost]
	public async Task<ActionResult<QuestionnaireResponseDto>> CreateQuestionnaireResponse([FromBody] CreateQuestionnaireResponseCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
