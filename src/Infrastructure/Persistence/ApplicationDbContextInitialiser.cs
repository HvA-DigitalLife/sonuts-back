using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Entities.Owned;
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
		foreach (var roleName in Enum.GetNames<Role>().Where(roleName => _roleManager.Roles.All(role => role.Name != roleName)))
		{
			await _roleManager.CreateAsync(new IdentityRole(roleName));
		}

		// Default users
		List<(ApplicationUser, string)> users = new()
		{
			(new ApplicationUser { UserName = "admin@local", Email = "admin@local" }, Role.Admin.ToString()),
			(new ApplicationUser { UserName = "participant@local", Email = "participant@local"}, Role.Participant.ToString() )
		};

		foreach (var user in users)
		{
			(ApplicationUser applicationUser, string roleName) = user;

			if (_userManager.Users.All(u => u.UserName != applicationUser.UserName))
			{
				await _userManager.CreateAsync(applicationUser, "Sonuts1!"); //TODO: Get default password from appsettings
				await _userManager.AddToRolesAsync(applicationUser, new[] { roleName });
			}
		}
		
		bool shouldSave = false;

		foreach (var contentType in Enum.GetNames<ContentType>())
		{
			if (await _context.Content.FirstOrDefaultAsync(content => content.Type.Equals(contentType)) is not null)
				continue;

			shouldSave = true;
			_context.Content.Add(new Content
			{
				Type = Enum.Parse<ContentType>(contentType),
				Title = "",
				Description = ""
			});
		}

		if (!_context.Categories.Any())
		{
			await _context.Categories.AddAsync(new Category
			{
				IsActive = true,
				Name = "Intake",
				Color = "3DA9DE",
				Questionnaire = new Questionnaire
				{
					Title = "Persoonlijke gegevens",
					Description = null,
					Questions = new List<Question>
					{
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is leeftijd?",
							Order = 0,
						},
						new()
						{
							Type = QuestionType.MultipleChoice,
							Text = "Wat is je geslacht?",
							Order = 1,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Text = "Man", Order = 0 },
								new() { Text = "Vrouw", Order = 1 },
								new() { Text = "Anders", Order = 2 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "In welk land ben je zelf geboren?",
							Order = 2
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "In welk land is jouw moeder geboren?",
							Order = 3
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "In welk land is jouw vader geboren?",
							Order = 4
						},
						new()
						{
							Id = Guid.Parse("7565096f-9c8e-4e62-bdc7-9cd73ade8349"),
							Type = QuestionType.MultipleOpen,
							Text = "Welke van de onderstaande beschijvingen past het beste bij jouw situatie?",
							Order = 5,
							MaxAnswers = 3,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Text = "Alleenstaand", Order = 0 },
								new() { Text = "Getrouwd", Order = 1 },
								new() { Text = "Samenwonend", Order = 2 },
								new() { Text = "Apart wonend", Order = 3 },
								new() { Text = "Weduwnaar", Order = 4 },
								new() { Text = "Gescheiden", Order = 5 },
								new() { Text = "Anders, namelijk:", Order = 6 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Welke andere situatie?",
							Order = 6,
							QuestionDependency = new QuestionDependency
							{
								QuestionId = Guid.Parse("7565096f-9c8e-4e62-bdc7-9cd73ade8349"),
								Operator = Operator.Equals,
								Value = "Anders, namelijk:"
							}
						},
						new()
						{
							Type = QuestionType.MultipleChoice,
							Text = "Heb je kinderen?",
							Order = 7,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Text = "Nee", Order = 0 },
								new() { Text = "Ja", Order = 1 }
							}
						},
						new()
						{
							Id = Guid.Parse("999d06ed-d0ca-46ff-8971-71193538db8d"),
							Type = QuestionType.MultipleChoice,
							Text = "Wat is de hoofste opleiding die jij hebt afgerond?",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Text = "Geen opleiding afgemaakt", Order = 0 },
								new() { Text = "Basisschool / lager beroepsonderwijs", Order = 1 },
								new() { Text = "VMBO / MAVO / MULO", Order = 3 },
								new() { Text = "MBO / MTS / MEAO / HAVO / VWO", Order = 4 },
								new() { Text = "HBO", Order = 5 },
								new() { Text = "Wetenschappelijk onderwijs / uni", Order = 6 },
								new() { Text = "Anders, Namelijk:", Order = 7 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Welke andere opleiding?",
							Order = 9,
							QuestionDependency = new QuestionDependency
							{
								QuestionId = Guid.Parse("999d06ed-d0ca-46ff-8971-71193538db8d"),
								Operator = Operator.Equals,
								Value = "Anders, namelijk:"
							}
						},
						new()
						{
							Id = Guid.Parse("e9efcf8e-955d-4017-9ab3-d57f2238cd02"),
							Type = QuestionType.MultipleOpen,
							Text = "Wat is jouw arbeidspositie?",
							Description = "Meerdere antwoorden mogelijk",
							Order = 10,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Text = "Werken - voltijds", Order = 0 },
								new() { Text = "Werken - deeltijds", Order = 1 },
								new() { Text = "Gepensioneerd", Order = 3 },
								new() { Text = "Huisman/vrouw", Order = 4 },
								new() { Text = "Student", Order = 5 },
								new() { Text = "Niet werkzaam en niet gepensioneerd", Order = 6 },
								new() { Text = "Blijvend arbeidsongeschikt / ziek", Order = 7 },
								new() { Text = "Anders, Namelijk:", Order = 8 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Welke andere arbeidspositie?",
							Order = 11,
							QuestionDependency = new QuestionDependency
							{
								QuestionId = Guid.Parse("e9efcf8e-955d-4017-9ab3-d57f2238cd02"),
								Operator = Operator.Equals,
								Value = "Anders, namelijk:"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is je lengte?",
							Description = "In centimeters",
							Order = 12
						},
						new()
						{
							Type = QuestionType.Decimal,
							Text = "Wat is jouw gewicht?",
							Description = "In kilogram",
							Order = 13
						}
					}
				}
			});
		}

		if (shouldSave) await _context.SaveChangesAsync();
	}
}
