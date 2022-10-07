using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.QuestionnaireResponses.Commands;
using Sonuts.Application.Logic.QuestionnaireResponses.Queries;

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

	/// <summary>
	/// Get questionnaire response for questionnaire 
	/// </summary>
	[HttpGet("{questionnaireId:guid}/QuestionnaireResponse")]
	public async Task<ActionResult<QuestionnaireResponseDto>> GetQuestionnaireResponseForQuestionnaire([FromQuery] Guid questionnaireId)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireResponseForQuestionnaireQuery
		{
			QuestionnaireId = questionnaireId
		}));
	}
}
