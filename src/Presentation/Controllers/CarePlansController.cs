using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.CarePlans.Commands;
using Sonuts.Application.Dtos;

namespace Sonuts.Presentation.Controllers;

public class CarePlansController : ApiControllerBase
{
	/// <summary>
	/// Create care plan
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPost]
	public async Task<ActionResult<CarePlanDto>> CreateCarePlan(CreateCarePlanCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
