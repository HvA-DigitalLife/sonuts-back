using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Categories;
using Sonuts.Application.Categories.Queries;

namespace Sonuts.Presentation.Controllers;

[AllowAnonymous]
public class CategoriesController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult<ICollection<CategoryDto>>> GetContentByType()
	{
		return Ok(await Mediator.Send(new GetCategoriesQuery()));
	}
}
