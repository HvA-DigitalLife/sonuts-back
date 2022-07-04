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
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
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
		bool shouldSave = false;

		// Default roles
		foreach (var roleName in Enum.GetNames<Role>().Where(roleName => _roleManager.Roles.All(role => role.Name != roleName)))
		{
			await _roleManager.CreateAsync(new IdentityRole(roleName));
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

			if (_userManager.Users.All(u => u.UserName != applicationUser.UserName))
			{
				await _userManager.CreateAsync(applicationUser, "Sonuts1!"); //TODO: Get default password from appsettings
				await _userManager.AddToRolesAsync(applicationUser, new[] { roleName });

				if (roleName.Equals(Role.Participant.ToString()))
				{
					shouldSave = true;
					await _context.Participants.AddAsync(new Participant { Id = Guid.Parse(applicationUser.Id) });
				}
			}
		}
		
		// Default Categories
		if (!await _context.Content.AnyAsync())
		{
			shouldSave = true;

			_context.Content.Add(new Content
			{
				Type = ContentType.Introduction,
				Title = "Welkom {name}",
				Subtitle = "Uitleg",
				Description = "Welkom bij de applicatie van Mensen in Beweging!\r\n\r\nDeze applicatie heeft als doel je te ondersteunen om gezonder te gaan eten en meer te gaan bewegen!"
			});
			_context.Content.Add(new Content
			{
				Type = ContentType.Intake,
				Title = "Intake",
				Subtitle = "Uitleg",
				Description = "De intake bestaat uit drie onderdelen. In totaal ben je ongeveer 30 minuten bezig met de intake. Er zijn vragen over je persoonlijkheid, voeding en beweging. Na deze intake wordt er samen met jou een plan  gemaakt."
			});
			_context.Content.Add(new Content
			{
				Type = ContentType.Themes,
				Title = "Intake",
				Subtitle = "Uitleg",
				Description = "De intake bestaat uit drie onderdelen. In totaal ben je ongeveer 30 minuten bezig met de intake. Er zijn vragen over je persoonlijkheid, voeding en beweging. Na deze intake wordt er samen met jou een plan  gemaakt."
			});
			_context.Content.Add(new Content
			{
				Type = ContentType.ThemeChoice,
				Title = "Hartelijk dank!",
				Subtitle = "Keuze thema’s",
				Description = "Zowel voor het onderdeel bewegen als voeding krijg je te zien op welke thema’s je al aan de richtlijn voldoet en waar nog ruimte is voor verbetering. Kies op de volgende pagina 3 themas waar je aan zou willen werken de komende tijd."
			});
			_context.Content.Add(new Content
			{
				Type = ContentType.Schedule,
				Title = "Planning",
				Subtitle = "Geplande activiteiten",
				Description = "Op de volgende pagina zie je welke activiteiten je voor komende week hebt gepland om je doelen."
			});
		}

		if (!await _context.Categories.AnyAsync())
		{
			shouldSave = true;
			await _context.Categories.AddAsync(new Category
			{
				IsActive = true,
				Name = "Intake",
				Color = "F6B042",
				Questionnaire = new Questionnaire
				{
					Title = "Intake",
					Description = "Persoonlijke gegevens",
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
								new() { Value = "Man", Order = 0 },
								new() { Value = "Vrouw", Order = 1 },
								new() { Value = "Anders", Order = 2 }
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
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Alleenstaand", Order = 0 },
								new() { Value = "Getrouwd", Order = 1 },
								new() { Value = "Samenwonend", Order = 2 },
								new() { Value = "Apart wonend", Order = 3 },
								new() { Value = "Weduwnaar", Order = 4 },
								new() { Value = "Gescheiden", Order = 5 },
								new() { Value = "Anders, namelijk:", Order = 6 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Welke andere situatie?",
							Order = 6,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("7565096f-9c8e-4e62-bdc7-9cd73ade8349"),
								Operator = Operator.Equals,
								Answer = "Anders, namelijk:"
							}
						},
						new()
						{
							Type = QuestionType.MultipleChoice,
							Text = "Heb je kinderen?",
							Order = 7,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Nee", Order = 0 },
								new() { Value = "Ja", Order = 1 }
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
								new() { Value = "Geen opleiding afgemaakt", Order = 0 },
								new() { Value = "Basisschool / lager beroepsonderwijs", Order = 1 },
								new() { Value = "VMBO / MAVO / MULO", Order = 3 },
								new() { Value = "MBO / MTS / MEAO / HAVO / VWO", Order = 4 },
								new() { Value = "HBO", Order = 5 },
								new() { Value = "Wetenschappelijk onderwijs / uni", Order = 6 },
								new() { Value = "Anders, Namelijk:", Order = 7 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Welke andere opleiding?",
							Order = 9,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("999d06ed-d0ca-46ff-8971-71193538db8d"),
								Operator = Operator.Equals,
								Answer = "Anders, namelijk:"
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
								new() { Value = "Werken - voltijds", Order = 0 },
								new() { Value = "Werken - deeltijds", Order = 1 },
								new() { Value = "Gepensioneerd", Order = 3 },
								new() { Value = "Huisman/vrouw", Order = 4 },
								new() { Value = "Student", Order = 5 },
								new() { Value = "Niet werkzaam en niet gepensioneerd", Order = 6 },
								new() { Value = "Blijvend arbeidsongeschikt / ziek", Order = 7 },
								new() { Value = "Anders, Namelijk:", Order = 8 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Welke andere arbeidspositie?",
							Order = 11,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("e9efcf8e-955d-4017-9ab3-d57f2238cd02"),
								Operator = Operator.Equals,
								Answer = "Anders, namelijk:"
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
			await _context.Categories.AddAsync(new Category
			{
				IsActive = true,
				Name = "Voeding",
				Color = "94BF31",
				Questionnaire = new Questionnaire
				{
					Id = Guid.Parse("08f26fc7-eb92-4a59-b15c-ff0fae8570a2"),
					Title = "Intake",
					Description = "Vragen over voeding",
					Questions = new List<Question>
					{
						new()
						{
							Type = QuestionType.MultipleOpen,
							Text = "Heeft u speciale voedingsgewoontes?",
							Description = "Meerdere antwoorden mogelijk",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Nee", Order = 0 },
								new() { Value = "Ik eet vegetarisch", Order = 1 },
								new() { Value = "Ik eet geen vlees, maar wel vis", Order = 2 },
								new() { Value = "Ik eet veganistisch", Order = 3 },
								new() { Value = "Ik eet geen varkensvlees", Order = 4 },
								new() { Value = "Ik eet geen koeienvlees", Order = 5 },
								new() { Value = "Ik eet flexitarisch", Order = 6 },
								new() { Value = "Ja, anders namelijk:", Order = 7 }
							}
						},
						new()
						{
							Type = QuestionType.Open,
							Text = "Speciale voedingsgewoontes",
							Order = 1,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("08f26fc7-eb92-4a59-b15c-ff0fae8570a2"),
								Operator = Operator.Equals,
								Answer = "Ja, anders namelijk:"
							}
						}
					}
				},
				Themes = new List<Theme>
				{
					new()
					{
						Name = "Groenten",
						Description = "Dagelijks ten minste 200 gram groente",
						Image = new Image(),
						FrequencyType = FrequencyType.Amount,
						FrequencyGoal = 1400,
						CurrentQuestion = "Hoe vaak in de week eet je al groente?",
						GoalQuestion = "Hoe vaak in de week wil je groente eten?",
						Activities = new List<Domain.Entities.Activity>
						{
							new()
							{
								Name = "Avondeten",
								Description = "Eet groente bij het avond eten",
								Image = new Image()
							},
							new()
							{
								Name = "Lunch",
								Description = "Eet groente bij de lunch",
								Image = new Image()
							},
							new()
							{
								Name = "Ontbijt",
								Description = "Eet groente bij het ontbijt",
								Image = new Image()
							},
							new()
							{
								Name = "Snack",
								Description = "Eet groente als snack",
								Image = new Image()
							}
						}
					}
				}
			});
			await _context.Categories.AddAsync(new Category
			{
				IsActive = true,
				Name = "Bewegen",
				Color = "3DA9DE",
				Questionnaire = new Questionnaire
				{
					Title = "Intake",
					Description = "Vragen over beweging",
					Questions = new List<Question>
					{
						new()
						{
							Id = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
							Type = QuestionType.MultipleChoice,
							Text = "Is er spraken van lopen van/naar werk?",
							Order = 0,
							AnswerOptions = new List<AnswerOption>
							{
								new()
								{
									Value = "Nee",
									Order = 0
								},
								new()
								{
									Value = "Ja",
									Order = 1
								}
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel dagen per week?",
							Order = 1,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
								Operator = Operator.Equals,
								Answer = "Ja"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 2,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
								Operator = Operator.Equals,
								Answer = "Ja"
							}
						},
						new()
						{
							Type = QuestionType.MultipleChoice,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 2,
							AnswerOptions = new List<AnswerOption>
							{
								new()
								{
									Value = "Langzaam",
									Order = 0
								},
								new()
								{
									Value = "Gemiddeld",
									Order = 1
								},
								new()
								{
									Value = "Snel",
									Order = 2
								}
							},
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
								Operator = Operator.Equals,
								Answer = "Ja"
							}
						}
					}
				},
				Themes = new List<Theme>
				{
					new()
					{
						Name = "Intensief bewegen",
						Description = "Minimaal 150 minuten per week bewegen",
						Image = new Image(),
						FrequencyType = FrequencyType.Minutes,
						FrequencyGoal = 150,
						CurrentQuestion = "Welke beweging doe je al?",
						GoalQuestion = "Welke beweging wil je nog meer doen?",
						Activities = new List<Domain.Entities.Activity>
						{
							new()
							{
								Name = "Geen",
								Description = "Geen beweging",
								Image = new Image()
							},
							new()
							{
								Name = "Fietsen",
								Description = "Minuten fietsen",
								Image = new Image()
							},
							new()
							{
								Name = "Wandelen",
								Description = "Minuten wandelen",
								Image = new Image()
							},
							new()
							{
								Name = "Gymnastiek",
								Description = "Minuten gymnastiek",
								Image = new Image()
							},
							new()
							{
								Name = "Zwemmen",
								Description = "Minuten zwemmen",
								Image = new Image()
							},
							new()
							{
								Name = "Dadminton",
								Description = "Minuten badminton",
								Image = new Image()
							}
						}
					}
				}
			});
		}

		if (shouldSave) await _context.SaveChangesAsync();
	}
}
