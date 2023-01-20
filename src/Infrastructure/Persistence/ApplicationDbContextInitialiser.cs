using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Persistence.Seeders;
using Sonuts.Application.Common.Interfaces.Fhir;

namespace Sonuts.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
	private readonly ILogger<ApplicationDbContextInitialiser> _logger;
	private readonly ApplicationDbContext _context;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	private readonly IFhirOptions _fhirOptions;

	private readonly ICategoryDao _categoryDao;
	private readonly IParticipantDao _participantDao;
	private readonly IQuestionnaireDao _questionnaireDao;
	private readonly IThemeDao _themeDao;
	private readonly IActivityDao _activityDao;

	public ApplicationDbContextInitialiser(
		ILogger<ApplicationDbContextInitialiser> logger, 
		ApplicationDbContext context, 
		UserManager<User> userManager, 
		RoleManager<IdentityRole> roleManager,
		IFhirOptions fhirOptions,
		ICategoryDao categoryDao,
		IParticipantDao participantDao,
		IQuestionnaireDao questionnaireDao,
		IThemeDao themeDao,
		IActivityDao activityDao)
	{
		_logger = logger;
		_context = context;
		_userManager = userManager;
		_roleManager = roleManager;
		_categoryDao = categoryDao;
		_participantDao = participantDao;
		_questionnaireDao = questionnaireDao;
		_themeDao = themeDao;
		_activityDao = activityDao;
		_fhirOptions = fhirOptions;
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
		await UserSeed.Seed(_userManager, _roleManager, _context, _fhirOptions, _participantDao);
		await ClientSeed.Seed(_context);
		await ContentSeed.Seed(_context);
		await CategorySeed.Seed(_context, _fhirOptions, _categoryDao, _questionnaireDao, _themeDao, _activityDao);
		await RecommendationRuleSeed.Seed(_context);
	}
}
