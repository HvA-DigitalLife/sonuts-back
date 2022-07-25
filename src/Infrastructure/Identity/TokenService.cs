using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Identity;

public class TokenService : ITokenService
{
	private readonly UserManager<User> _userManager;
	private readonly IApplicationDbContext _context;
	private readonly IConfiguration _configuration;
	private readonly IDateTime _dateTime;

	public TokenService(UserManager<User> userManager, IApplicationDbContext context, IConfiguration configuration, IDateTime dateTime)
	{
		_userManager = userManager;
		_context = context;
		_configuration = configuration;
		_dateTime = dateTime;
	}

	public async Task<string> CreateAccessTokenAsync(string username)
	{
		var user = await _userManager.FindByNameAsync(username);

		if (user is null) throw new ForbiddenAccessException();
		
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.NameIdentifier, user.Id)
			}),
			Expires = _dateTime.Now.AddMilliseconds(_configuration.GetValue<int>("Authentication:TokenDuration")),
			IssuedAt = _dateTime.Now,
			Issuer = _configuration["Authentication:Issuer"],
			Audience = _configuration["Authentication:Audience"],
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecurityKey"])), SecurityAlgorithms.HmacSha256Signature)
		};

		foreach (var role in await _userManager.GetRolesAsync(user))
		{
			tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
		}

		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}

	public async Task<string> CreateRefreshTokenAsync(string username, Guid clientId)
	{
		var user = await _userManager.FindByNameAsync(username);

		if (user is null) throw new ForbiddenAccessException();

		var client = _context.Clients.FirstOrDefault(client => client.Id.Equals(clientId));

		if (client is null) throw new NotFoundException(nameof(Client), clientId);

		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		var token = Convert.ToBase64String(randomNumber);

		await _context.RefreshTokens.AddAsync(new RefreshToken
		{
			Token = token,
			IssuedAt = _dateTime.Now,
			User = user,
			Client = client
		});
		
		_context.RefreshTokens.RemoveRange(await _context.RefreshTokens.Where(refreshToken => refreshToken.Client.Id.Equals(clientId)).ToListAsync());
		await _context.SaveChangesAsync();

		return token;
	}

	public string CreateInternalNetworkAccessToken()
	{
		var now = _dateTime.Now;
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.Role, "InternalNetwork")
			}),
			Expires = now.AddMinutes(1),
			IssuedAt = now,
			Issuer = _configuration["Authentication:Issuer"],
			Audience = _configuration["Authentication:Audience"],
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecurityKey"])), SecurityAlgorithms.HmacSha256Signature)
		};

		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}
}
