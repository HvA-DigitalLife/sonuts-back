using Sonuts.Application.Common.Models;

namespace Sonuts.Application.Common.Interfaces;

public interface IIdentityService
{
	Task<string?> GetIdAsync(string userName);

	Task<string?> GetUserNameAsync(string userId);

	Task<IList<string>> GetRolesAsync(string userId);

	Task<bool> IsInRoleAsync(string userId, string role);

	Task<bool> IsUser(string userId);

	Task<Result> AddToRole(string userId, string role);

	Task<bool> AuthorizeAsync(string userId, string policyName);

	Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

	Task<Result> DeleteUserAsync(string userId);

	Task<bool> CheckPasswordAsync(string username, string password);

	Task<bool> CheckRefreshTokenAsync(string username, string token, Guid clientId);
}
