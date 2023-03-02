using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.CarePlans.Commands;
using Sonuts.Application.Logic.CarePlans.Queries;

namespace Sonuts.Presentation.Controllers;

public class CarePlansController : ApiControllerBase
{
	/// <summary>
	/// Create care plan
	/// </summary>
	[Authorize(Roles = "Participant")]
	[HttpPost]
	public async Task<ActionResult<CarePlanDto>> CreateCarePlan(CreateCarePlanCommand command, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(command, cancellationToken));
	}
}
