using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;

namespace Sonuts.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
	private readonly IAuthorizationService _authorizationService;
	private readonly IConfiguration _configuration;

	public IdentityService(
		UserManager<ApplicationUser> userManager,
		IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
		IAuthorizationService authorizationService,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
		_authorizationService = authorizationService;
		_configuration = configuration;
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
		var user = new ApplicationUser
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

	public async Task<bool> AuthorizeAsync(string userId, string policyName)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		if (user == null)
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

	public async Task<Result> DeleteUserAsync(ApplicationUser user)
	{
		var result = await _userManager.DeleteAsync(user);

		return result.ToApplicationResult();
	}


	public async Task<string> CreateAccessTokenAsync(string userId)
	{
		var now = DateTime.Now;
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.Name, userId)
			}),
			Expires = now.AddMilliseconds(int.Parse(_configuration["Authentication:TokenDuration"])),
			IssuedAt = now,
			Issuer = "Beyco",
			Audience = "Beyco",
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecurityKey"])), SecurityAlgorithms.HmacSha256Signature)
		};

		foreach (var role in await GetRolesAsync(userId))
		{
			tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
		}

		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}

	public string CreateRefreshTokenAsync(string userId)
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		var token = Convert.ToBase64String(randomNumber);
		return token;
	}
}
