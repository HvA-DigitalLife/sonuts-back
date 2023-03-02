using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Logic.Categories.Models;
using Sonuts.Application.Logic.Categories.Queries;

namespace Sonuts.Presentation.Controllers;

public class CategoriesController : ApiControllerBase
{
	/// <summary>
	/// Get all active categories
	/// </summary>
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet]
	public async Task<ActionResult<ICollection<CategoriesWithRecommendationsVm>>> GetCategories(CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(new GetCategoriesQuery(), cancellationToken));
	}
}
