namespace Sonuts.Application.Common.Interfaces;

public interface ICurrentUserService
{
	string? UserId { get; }

	/// <summary>
	/// Gets user id. Throws if user has no id.
	/// </summary>
	/// <exception cref="UnauthorizedAccessException"></exception>
	string AuthorizedUserId { get; }
}
