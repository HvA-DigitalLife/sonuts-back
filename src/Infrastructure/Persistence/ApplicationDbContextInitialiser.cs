using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Persistence.Seeders;

namespace Sonuts.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
	private readonly ILogger<ApplicationDbContextInitialiser> _logger;
	private readonly ApplicationDbContext _context;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	private readonly ICategoryDao _categoryDao;
	private readonly IQuestionnaireDao _questionnaireDao;

	public ApplicationDbContextInitialiser(
		ILogger<ApplicationDbContextInitialiser> logger, 
		ApplicationDbContext context, 
		UserManager<User> userManager, 
		RoleManager<IdentityRole> roleManager, 
		ICategoryDao categoryDao,
		IQuestionnaireDao questionnaireDao)
	{
		_logger = logger;
		_context = context;
		_userManager = userManager;
		_roleManager = roleManager;
		_categoryDao = categoryDao;
		_questionnaireDao = questionnaireDao;
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
		await UserSeed.Seed(_userManager, _roleManager, _context);
		await ClientSeed.Seed(_context);
		await ContentSeed.Seed(_context);
		await CategorySeed.Seed(_context, _categoryDao, _questionnaireDao);
	}
}
