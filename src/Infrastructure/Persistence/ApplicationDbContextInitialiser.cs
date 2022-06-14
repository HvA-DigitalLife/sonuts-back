using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;
using Sonuts.Infrastructure.Identity;

namespace Sonuts.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
	private readonly ILogger<ApplicationDbContextInitialiser> _logger;
	private readonly ApplicationDbContext _context;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_logger = logger;
		_context = context;
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task InitialiseAsync()
	{
		try
		{
			if (_context.Database.IsNpgsql())
			{
				await _context.Database.MigrateAsync();
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initializing the database.");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database.");
			throw;
		}
	}

	public async Task TrySeedAsync()
	{
		// Default roles
		var administratorRole = new IdentityRole("Administrator");

		if (_roleManager.Roles.All(role => role.Name != administratorRole.Name)) await _roleManager.CreateAsync(administratorRole);

		// Default users
		var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

		if (_userManager.Users.All(u => u.UserName != administrator.UserName))
		{
			await _userManager.CreateAsync(administrator, "Administrator1!");
			await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
		}
		
		bool shouldSave = false;

		foreach (var contentType in Enum.GetValues(typeof(ContentType)))
		{
			if (await _context.Content.FirstOrDefaultAsync(content => content.Type.Equals(contentType)) is null)
			{
				shouldSave = true;
				_context.Content.Add(new Content
				{
					Type = Enum.Parse<ContentType>(contentType.ToString()!),
					Title = "",
					Description = ""
				});
			}
		}
		
		if (shouldSave) await _context.SaveChangesAsync();
	}
}
