using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.CarePlans.Queries;
using Sonuts.Application.Logic.Participants.Commands;
using Sonuts.Application.Logic.Participants.Queries;
using Sonuts.Application.Logic.QuestionnaireResponses.Models;
using Sonuts.Application.Logic.QuestionnaireResponses.Queries;

namespace Sonuts.Presentation.Controllers;

public class ParticipantsController : ApiControllerBase
{
	/// <summary>
	/// Get all participants
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet]
	public async Task<ActionResult<IEnumerable<OverviewParticipantDto>>> GetAllParticipants()
	{
		return Ok(await Mediator.Send(new GetAllParticipantsQuery()));
	}

	/// <summary>
	/// Export all participants
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("csv")]
	public async Task<ActionResult> GetParticipantsCsv()
	{
		return File(await Mediator.Send(new GetParticipantsCsvQuery()), "text/csv", $"Participants-{DateOnly.FromDateTime(DateTime.Now):yyyy-MM-dd}");
	}

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
