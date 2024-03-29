using Microsoft.AspNetCore.Identity;
using Sonuts.Application.Common.Models;

namespace Sonuts.Infrastructure.Identity;

public static class IdentityResultExtensions
{
	public static Result ToApplicationResult(this IdentityResult result)
	{
		return result.Succeeded
			? Result.Success()
			: Result.Failure(result.Errors.Select(e => e.Description));
	}
}
