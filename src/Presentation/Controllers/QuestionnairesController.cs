using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.QuestionnaireResponses.Queries;
using Sonuts.Application.Logic.Questionnaires.Queries;

namespace Sonuts.Presentation.Controllers;

public class QuestionnairesController : ApiControllerBase
{
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet]
	public async Task<ActionResult<QuestionnaireDto>> GetQuestionnaireByCategory([FromQuery] Guid categoryId)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireByCategoryQuery
		{
			CategoryId = categoryId
		}));
	}

	/// <summary>
	/// Get questionnaire response for questionnaire 
	/// </summary>
	[HttpGet("{questionnaireId:guid}/QuestionnaireResponse")]
	public async Task<ActionResult<QuestionnaireResponseDto>> GetQuestionnaireResponseForQuestionnaire([FromRoute] Guid questionnaireId)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireResponsesForQuestionnaireQuery
		{
			QuestionnaireId = questionnaireId
		}));
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("{questionnaireId:guid}/QuestionnaireResponse/Csv")]
	public async Task<ActionResult> GetFile(Guid questionnaireId)
	{
		var file = await Mediator.Send(new ExportQuestionnaireResponsesQuery
		{
			QuestionnaireId = questionnaireId
		});

		return File(file.Content, file.ContentType, file.FileName);
	}
}
