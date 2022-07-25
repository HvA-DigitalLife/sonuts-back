using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonuts.Application.Categories.Models;
using Sonuts.Application.Categories.Queries;
using Sonuts.Application.Dtos;

namespace Sonuts.Presentation.Controllers;

public class CategoriesController : ApiControllerBase
{
	/// <summary>
	/// Get all active categories
	/// </summary>
	[Authorize(Roles = "Admin, Participant")]
	[HttpGet]
	public async Task<ActionResult<ICollection<CategoriesWithRecommendationsVm>>> GetCategories()
	{
		return Ok(await Mediator.Send(new GetCategoriesQuery()));
	}
}
