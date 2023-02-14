using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sonuts.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("Api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
	private ISender? _mediator;

	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
