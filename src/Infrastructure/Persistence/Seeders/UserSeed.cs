using Microsoft.AspNetCore.Identity;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal class UserSeed
{
	internal static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IApplicationDbContext context)
	{
		var shouldSave = false;

		// Default roles
		foreach (var roleName in Enum.GetNames<Role>().Where(roleName => roleManager.Roles.All(role => role.Name != roleName)))
		{
			await roleManager.CreateAsync(new IdentityRole(roleName));
		}

		// Default users
		List<(User, string)> users = new()
		{
			(new User { UserName = "admin@local", Email = "admin@local" }, Role.Admin.ToString()),
			(new User { UserName = "participant@local", Email = "participant@local"}, Role.Participant.ToString() )
		};

		foreach (var user in users)
		{
			(User applicationUser, string roleName) = user;

			if (userManager.Users.All(u => u.UserName != applicationUser.UserName))
			{
				await userManager.CreateAsync(applicationUser, "Sonuts1!"); //TODO: Get default password from appsettings
				await userManager.AddToRolesAsync(applicationUser, new[] { roleName });

				if (roleName.Equals(Role.Participant.ToString()))
				{
					shouldSave = true;
					context.Participants.Add(new Participant
					{
						Id = Guid.Parse(applicationUser.Id),
						FirstName = "FirstName",
						LastName = "LastName"
					});
				}
			}
		}

		if (shouldSave) await context.SaveChangesAsync();
	}
}
