using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly UserManager<User> _userManager;
	private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
	private readonly IAuthorizationService _authorizationService;
	private readonly IApplicationDbContext _context;

	public IdentityService(
		UserManager<User> userManager,
		IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
		IAuthorizationService authorizationService,
		IApplicationDbContext context)
	{
		_userManager = userManager;
		_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
		_authorizationService = authorizationService;
		_context = context;
	}

	public async Task<string> GetIdAsync(string userName)
	{
		var user = await _userManager.Users.FirstAsync(u => u.NormalizedUserName == userName.ToUpper());

		return user.Id;
	}

	public async Task<string> GetUserNameAsync(string userId)
	{
		var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

		return user.UserName;
	}

	public async Task<IList<string>> GetRolesAsync(string userId)
	{
		var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId));

		return user is not null
			? await _userManager.GetRolesAsync(user)
			: new List<string>();
	}

	public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
	{
		var user = new User
		{
			UserName = userName,
			Email = userName,
		};

		var result = await _userManager.CreateAsync(user, password);

		return (result.ToApplicationResult(), user.Id);
	}

	public async Task<bool> IsInRoleAsync(string userId, string role)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		return user != null && await _userManager.IsInRoleAsync(user, role);
	}

	public async Task<bool> IsUser(string userId)
	{
		var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

		return user is not null;
	}

	public async Task<Result> AddToRole(string userId, string role)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException(nameof(User), userId);

		var result = await _userManager.AddToRoleAsync(user, role);

		return result.ToApplicationResult();
	}

	public async Task<bool> AuthorizeAsync(string userId, string policyName)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		if (user is null)
		{
			return false;
		}

		var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

		var result = await _authorizationService.AuthorizeAsync(principal, policyName);

		return result.Succeeded;
	}

	public async Task<Result> DeleteUserAsync(string userId)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		return user != null ? await DeleteUserAsync(user) : Result.Success();
	}

	public async Task<Result> DeleteUserAsync(User user)
	{
		var result = await _userManager.DeleteAsync(user);

		return result.ToApplicationResult();
	}

	public async Task<bool> CheckPasswordAsync(string username, string password)
	{
		var user = await _userManager.FindByNameAsync(username);

		return user is not null && await _userManager.CheckPasswordAsync(user, password);
	}

	public async Task<bool> CheckRefreshTokenAsync(string username, string token, Guid clientId)
	{
		var refreshToken = await _context.RefreshTokens
			.Include(rt => rt.User)
			.Include(rt => rt.Client)
			.FirstOrDefaultAsync(rt => rt.Token.Equals(token));

		return refreshToken is not null && refreshToken.User.NormalizedUserName.Equals(username.ToUpper()) && refreshToken.Client.Id.Equals(clientId);
	}
}
