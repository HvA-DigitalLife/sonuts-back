using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.QuestionnaireResponses.Queries;
using Sonuts.Application.Logic.Questionnaires.Queries;

namespace Sonuts.Presentation.Controllers;

public class QuestionnairesController : ApiControllerBase
{
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet("{categoryId:guid}")]
	public async Task<ActionResult<QuestionnaireDto>> GetQuestionnaireByCategory([FromQuery] Guid categoryId)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireByCategoryQuery { CategoryId = categoryId }));
	}

	/// <summary>
	/// Get questionnaire response for questionnaire 
	/// </summary>
	[HttpGet("{questionnaireId:guid}/QuestionnaireResponse")]
	public async Task<ActionResult<QuestionnaireResponseDto>> GetQuestionnaireResponseForQuestionnaire([FromRoute] Guid questionnaireId)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireResponseForQuestionnaireQuery
		{
			QuestionnaireId = questionnaireId
		}));
	}
}
