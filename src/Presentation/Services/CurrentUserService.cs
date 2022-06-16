using System.Security.Claims;
using Sonuts.Application.Common.Interfaces;

namespace Sonuts.Presentation.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? UserId => _httpContextAccessor.HttpContext?.User.Identity?.Name;

	public string AuthorizedUserId => _httpContextAccessor.HttpContext?.User.Identity?.Name ??
	                                  throw new UnauthorizedAccessException();
}
