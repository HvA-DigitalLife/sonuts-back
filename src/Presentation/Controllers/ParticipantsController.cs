using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Participants.Commands;

namespace Sonuts.Presentation.Controllers;

public class ParticipantsController : ApiControllerBase
{
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
