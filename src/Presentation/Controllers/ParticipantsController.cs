using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.CarePlans.Queries;
using Sonuts.Application.Logic.Participants.Commands;
using Sonuts.Application.Logic.Participants.Queries;
using Sonuts.Application.Logic.QuestionnaireResponses.Models;
using Sonuts.Application.Logic.QuestionnaireResponses.Queries;
using Sonuts.Application.Logic.TinyHabits.Commands;
using Sonuts.Application.Logic.TinyHabits.Queries;

namespace Sonuts.Presentation.Controllers;

public class ParticipantsController : ApiControllerBase
{
	/// <summary>
	/// Get all participants
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet]
	public async Task<ActionResult<IEnumerable<ParticipantRecord>>> GetAllParticipants()
	{
		return Ok(await Mediator.Send(new GetAllParticipantsQuery()));
	}

	/// <summary>
	/// Export all participants
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpGet("Csv")]
	public async Task<ActionResult> ExportParticipants(CancellationToken cancellationToken)
	{
		var file = await Mediator.Send(new ExportParticipantsQuery(), cancellationToken);

		return File(file.Content, file.ContentType, file.FileName);
	}

	/// <summary>
	/// Get the currently logged in participant
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpGet("Current")]
	public async Task<ActionResult<ParticipantDto>> GetCurrentParticipant(CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetCurrentParticipantQuery(), cancellationToken));
	}

	/// <summary>
	/// Update the password for the participant
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPut("UpdatePassword")]
	public async Task<ActionResult<ChangePasswordDto>> UpdatePassword(UpdatePasswordForParticipantCommand command, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(command, cancellationToken));
	}

	/// <summary>
	/// Get all questionnaire responses for participant
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpGet("{participantId:guid}/QuestionnaireResponses")]
	public async Task<ActionResult<IList<QuestionnaireResponseVm>>> GetQuestionnaireResponses(Guid participantId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetQuestionnaireResponsesForParticipantQuery
		{
			ParticipantId = participantId
		}, cancellationToken));
	}

	/// <summary>
	/// Get 
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Participant")]
	[HttpGet("{participantId:guid}/CarePlan")]
	public async Task<ActionResult<CarePlanDto>> GetCurrentCarePlan(Guid participantId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetCurrentCarePlanQuery
		{
			ParticipantId = participantId
		}, cancellationToken));
	}

	/// <summary>
	/// Get all tiny habits for the participant
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Participant")]
	[HttpGet("{participantId:guid}/TinyHabits")]
	public async Task<ActionResult<IList<TinyHabitDto>>> GetTinyHabitsOverview(Guid participantId, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetTinyHabitsOverviewForParticipantQuery
		{
			ParticipantId = participantId
		}, cancellationToken));
	}

	/// <summary>
	/// Create tiny habit for a participant
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPost("TinyHabit")]
	public async Task<ActionResult<TinyHabitDto>> CreateTinyHabit(CreateTinyHabitCommand command, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(command, cancellationToken));
	}

	/// <summary>
	/// Create users that can partake in the program (Admin token required)
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult<ParticipantDto>> CreateParticipant(CreateParticipantCommand command, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(command, cancellationToken));
	}
}
