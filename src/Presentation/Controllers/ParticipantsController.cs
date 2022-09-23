using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.CarePlans.Queries;
using Sonuts.Application.Dtos;
using Sonuts.Application.Participants.Commands;
using Sonuts.Application.Participants.Queries;
using Sonuts.Application.QuestionnaireResponses.Models;
using Sonuts.Application.QuestionnaireResponses.Queries;

namespace Sonuts.Presentation.Controllers;

public class ParticipantsController : ApiControllerBase
{
	/// <summary>
	/// Get the currently logged in participant
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpGet("current")]
	public async Task<ActionResult<ParticipantDto>> GetCurrentParticipant()
	{
		return Ok(await Mediator.Send(new GetCurrentParticipantQuery()));
	}

	/// <summary>
	/// Get all questionnaire responses for participant
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpGet("{participantId:guid}/QuestionnaireResponses")]
	public async Task<ActionResult<IList<QuestionnaireResponseVm>>> GetQuestionnaireResponses(Guid participantId)
	{

		return Ok(await Mediator.Send(new GetQuestionnaireResponsesForParticipantQuery
		{
			ParticipantId = participantId
		}));
	}

	/// <summary>
	/// Get 
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Participant")]
	[HttpGet("{participantId:guid}/CarePlan")]
	public async Task<ActionResult<CarePlanDto>> GetCurrentCarePlan(Guid participantId)
	{
		return Ok(await Mediator.Send(new GetCurrentCarePlanQuery
		{
			ParticipantId = participantId
		}));
	}

	/// <summary>
	/// Create users that can partake in the program (Admin token required)
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult<ParticipantDto>> CreateParticipant(CreateParticipantCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
