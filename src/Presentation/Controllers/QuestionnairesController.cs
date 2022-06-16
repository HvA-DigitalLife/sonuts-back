using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Questionnaires.Queries;

namespace Sonuts.Presentation.Controllers;

public class QuestionnairesController : ApiControllerBase
{
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet("{categoryId:guid}")]
	public async Task<ActionResult<QuestionnaireDto>> GetQuestionnaireByCategory(Guid categoryId)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireByCategoryQuery { CategoryId = categoryId }));
	}
}
