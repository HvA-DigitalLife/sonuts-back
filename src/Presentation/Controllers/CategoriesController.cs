using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Categories;
using Sonuts.Application.Categories.Queries;

namespace Sonuts.Presentation.Controllers;

public class CategoriesController : ApiControllerBase
{
	/// <summary>
	/// Get all active categories
	/// </summary>
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet]
	public async Task<ActionResult<ICollection<CategoryDto>>> GetCategories()
	{
		return Ok(await Mediator.Send(new GetCategoriesQuery()));
	}
}
