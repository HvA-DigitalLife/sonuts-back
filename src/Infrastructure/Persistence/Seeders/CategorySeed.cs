using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal static class CategorySeed
{
	private static readonly Guid IntakeId = new("791dac69-f1af-47e2-8d4d-e89a83e54e72");
	private static readonly Guid VoedingId = new("a5997737-7f28-4f5f-92fc-6054023b1248");
	private static readonly Guid BewegenId = new("33c34b68-0925-4af4-a612-11503c87208f");

	internal static async Task Seed(IApplicationDbContext context /*, IFhirOptions fhirOptions, ICategoryDao categoryDao, IQuestionnaireDao questionnaireDao, IThemeDao themeDao*/)
	{
		List<Category> categories = new();

		if (await context.Categories.FindAsync(IntakeId) is null)
			categories.Add(new Category
			{
				Id = IntakeId,
				IsActive = true,
				Name = "Intake",
				Color = "F6B042",
				Questionnaire = new Questionnaire
				{
					Title = "Persoonlijke gegevens",
					Description = "De volgende vragen gaan over uzelf.",
					Questions = new List<Question>
					{
						new()
						{
							Type = QuestionType.Choice,
							Text = "Wat is uw geslacht?",
							Order = 0,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Man", Value = "Man", Order = 0 },
								new() { Name = "Vrouw", Value = "Vrouw", Order = 1 },
								new() { Name = "Anders", Value = "Anders", Order = 2 },
								new() { Name = "Zeg ik liever niet", Value = "Zeg ik liever niet", Order = 3 }
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw leeftijd?",
							Order = 1,
							Min = 1
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw lengte?",
							Description = "In centimeters",
							Order = 2,
							Min = 1
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw gewicht?",
							Description = "In kilogram",
							Order = 3,
							Min = 1
						}
					}
				}
			});

		if (await context.Categories.FindAsync(VoedingId) is null)
			categories.Add(new Category
			{
				Id = VoedingId,
				IsActive = true,
				Name = "Voeding",
				Color = "94BF31",
				Questionnaire = new Questionnaire
				{
					Title = "Vragen over voeding",
					Description = "Vragen over voeding",
					Questions = new List<Question>
					{
						new() // 1A
						{
							Type = QuestionType.MultiOpenChoice,
							Text = "Heeft u speciale voedingsgewoontes?",
							Description = "Meerdere antwoorden mogelijk",
							Order = 0,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Nee", Value = "Nee", Order = 0 },
								new() { Name = "Ik eet vegetarisch (geen vis, geen vlees)", Value = "Ik eet vegetarisch (geen vis, geen vlees)", Order = 1 },
								new() { Name = "Ik eet geen vlees, maar wel vis", Value = "Ik eet geen vlees, maar wel vis", Order = 2 },
								new() { Name = "Ik eet veganistisch", Value = "Ik eet veganistisch", Order = 3 },
								new() { Name = "Ik eet geen varkensvlees", Value = "Ik eet geen varkensvlees", Order = 4 },
								new() { Name = "Ik eet geen koeienvlees", Value = "Ik eet geen koeienvlees", Order = 5 },
								new() { Name = "Ik eet flexitarisch", Value = "Ik eet flexitarisch", Order = 6 }
							},
							OpenAnswerLabel = "Ja, anders namelijk"
						},
						new() // 1B
						{
							Id = new Guid("eff4185b-c2ad-4746-8802-7ec3b00985d4"),
							Type = QuestionType.MultiOpenChoice,
							Text = "Volgt u een speciaal dieet?",
							Description = "Meerdere antwoorden mogelijk",
							Order = 1,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Glutenvrij", Value = "Glutenvrij", Order = 0 },
								new() { Name = "Lactosebeperkt/lactosevrij", Value = "Lactosebeperkt/lactosevrij", Order = 1 },
								new() { Name = "Zoutbeperkt", Value = "Zoutbeperkt", Order = 2 },
								new() { Name = "Ik ben allergisch voor 1 of meerdere voedingsmiddelen (e.g. pinda’s, schaaldieren)", Value = "Ik ben allergisch voor 1 of meerdere voedingsmiddelen (e.g. pinda’s, schaaldieren)", Order = 3 },
							},
							OpenAnswerLabel = "Anders, namelijk"
						},
						new()
						{
							Type = QuestionType.String,
							Text = "Indien ja; welke",
							Order = 2,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("eff4185b-c2ad-4746-8802-7ec3b00985d4"),
								Operator = Operator.Equals,
								Answer = "Ik ben allergisch voor 1 of meerdere voedingsmiddelen (e.g. pinda’s, schaaldieren)"
							}
						},
						new() // 2A
						{
							Type = QuestionType.Choice,
							Text = "Op gemiddeld hoeveel dagen per week eet u drie hoofdmaaltijden?",
							Description = "Ontbijt, lunch, avondmaaltijd",
							Order = 3,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 3A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u, gemiddeld gezien, fruit?",
							Order = 4,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 3B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties fruit eet u gemiddeld op een dag dat u fruit eet?",
							Description = "Een portie fruit is bijvoorbeeld: een appel, een banaan, twee mandarijntjes, twee kiwi’s, een dessertschaaltje gesneden of klein (bessen, aardbeien) fruit. U mag hierbij vruchtensap niet meerekenen.",
							Order = 5,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 },
								new () { Name = "Meer porties per dag", Value = "10", Order = 6 }
							}
						},
						new() // 4A
						{
							Id = new Guid("c05d9393-328c-4e54-a504-7330445fd8cf"),
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u, gemiddeld gezien, groente of rauwkost?",
							Description = "Zowel verse, diepvries als groente in conservenblik tellen mee.",
							Order = 6,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 4B
						{
							Id = new Guid("d14f00d4-32f2-4a22-8829-7d7ed86a127c"),
							Type = QuestionType.Choice,
							Text = "Hoeveel opscheplepels/porties groente of rauwkost (circa 50 gram) eet u gemiddeld op een dag dat u groente eet?",
							Order = 7,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 5A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per maand eet u, gemiddeld gezien, peulvruchten?",
							Description = "Peulvruchten zijn bijvoorbeeld bruine bonen, witte bonen, kapucijners, kidney(nier)bonen, linzen of kikkererwten. Reken doperwten, tuinbonen en sperziebonen niet mee, deze moeten bij groente worden ingevuld.",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "Een keer per maand", Value = "1", Order = 1 },
								new () { Name = "2-3 keer per maand", Value = "2", Order = 2 },
								new () { Name = "1 keer per week", Value = "4", Order = 3 },
								new () { Name = "2-3 keer per week", Value = "8", Order = 4 },
								new () { Name = "Meer dan 3 keer per week", Value = "15", Order = 5 }
							}
						},
						new() // 5B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel peulvruchten eet u op ’een dag dat u peulvruchten eet?",
							Description = "Opscheplepels (circa 50 gram) per dag (een portie = 200 gram, dus 4 opscheplepels).",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 6.1A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld witte rijst of witte deegwaren?",
							Description = "Macaroni, mie, spaghetti, witte couscous, gierst.",
							Order = 9,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 6.1B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel opscheplepels witte rijst of witte deegwaren eet u op zo’n dag?",
							Description = "Een opscheplepel staat gelijk aan 50 gram.",
							Order = 10,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 6.2A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld volkoren deegwaren?",
							Description = "(volkoren macaroni, spaghetti, volkoren couscous, bulgur, quinoa etc), zilvervliesrijst.",
							Order = 11,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 6.2B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel opscheplepels volkoren deegwaren eet u op zo’n dag?",
							Description = "Een opscheplepel staat gelijk aan 50 gram.",
							Order = 12,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 6.3A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld witte graanproducten?",
							Description = "(wit brood, crackers, beschuit, cornflakes etc.)",
							Order = 13,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 6.3B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel stuks witte graanproducten eet u op zo’n dag?",
							Order = 14,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 6.4A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld volkoren graanproducten?",
							Description = "Volkorenbrood, volkoren beschuit, volkoren crackers, tarwe zemelen, havermout of roggebrood.",
							Order = 15,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 6.4B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel stuks volkoren graanproducten eet u op zo’n dag?",
							Order = 16,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 7A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld ongezouten pinda’s of noten?",
							Description = "100% pindakaas of notenspread mag u hierbij meerekenen.",
							Order = 17,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 7B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel handjes noten of pinda’s eet u op zo’n dag?",
							Description = "kleine  handjes (circa 15 gram) per dag.",
							Order = 18,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "0.5", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "7", Value = "7", Order = 7 },
								new () { Name = "Meer", Value = "10", Order = 7 }
							}
						},
						new() // 8.1A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week drinkt u gemiddeld (karne)melk of sojamelk?",
							Order = 19,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 8.1B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel glazen (karne)melk of sojamelk drinkt u op zo’n dag?",
							Description = "Glazen/beker (200ml).",
							Order = 20,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 8.2A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week drinkt u gemiddeld chocolademelk, vla of vruchtendrank?",
							Order = 21,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 8.2B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel glazen chocolademelk, vla of vruchtendrank drinkt u op zo’n dag?",
							Description = "Glazen/beker (200ml).",
							Order = 22,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 8.3A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u kwark of skyr?",
							Order = 23,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 8.3B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel schaaltjes kwark of skyr eet u op zo’n dag?",
							Description = "Schaaltjes (150ml).",
							Order = 24,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 8.4A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u yoghurt, vruchtenyoghurt, vla etc?",
							Order = 25,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 8.4B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel schaaltjes yoghurt, vruchtenyoghurt, vla etc eet u op zo’n dag?",
							Description = "Schaaltjes (150ml).",
							Order = 26,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 9A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld magere vis?",
							Description = "Magere vis is bijvoorbeeld forel, kabeljauw, koolvis, pangasius, schelvis, schol, tilapia, tong, tonijn of wijting.",
							Order = 27,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 9B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties magere vis eet u gemiddeld op zo’n dag?",
							Order = 28,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 9C
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld vette vis?",
							Description = "Vette vis is bijvoorbeeld bokking, haring, heilbot, makreel, paling, sardines, sprot filet of zalm.",
							Order = 29,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 9D
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties vette vis eet u gemiddeld op zo’n dag?",
							Order = 30,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 10A
						{
							Type = QuestionType.Choice,
							Text = "Met welke soorten boter besmeert u meestal uw brood, knäckebröd, cracker of beschuit?",
							Order = 31,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Ik gebruik meestal geen boter", Value = "Ik gebruik meestal geen boter", Order = 0 },
								new () { Name = "Bijna altijd met (dieet) halvarine, (dieet)margarine", Value = "Bijna altijd met (dieet) halvarine, (dieet)margarine", Order = 1 },
								new () { Name = "Bijna altijd met (halfvolle) roomboter", Value = "Bijna altijd met (halfvolle) roomboter", Order = 2 },
								new () { Name = "Met zowel (dieet) halvarine, (dieet)margarine als (halfvolle) roomboter", Value = "Met zowel (dieet) halvarine, (dieet)margarine als (halfvolle) roomboter", Order = 3 }
							}
						},
						new() // 10B
						{
							Type = QuestionType.MultiChoice,
							Text = "Welk(e) soort(en) vet gebruikt u voor de bereiding van de warme maaltijd?",
							Description = "Hierbij zijn meerdere antwoorden mogelijk.",
							Order = 32,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Ik gebruik meestal geen boter, margarine, olie of andere bakproducten", Value = "Ik gebruik meestal geen boter, margarine, olie of andere bakproducten", Order = 0 },
								new () { Name = "Meestal roomboter", Value = "Meestal roomboter", Order = 1 },
								new () { Name = "Meestal margarine of bakproduct uit een pakje", Value = "Meestal margarine of bakproduct uit een pakje", Order = 2 },
								new () { Name = "Meestal margarine of bakproduct uit een fles", Value = "Meestal margarine of bakproduct uit een fles", Order = 3 },
								new () { Name = "Olie", Value = "Olie", Order = 4 }
							}
						},
						new() // 11.1A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld rood vlees?",
							Description = "Rood vlees is bijvoorbeeld vlees van het rund, varken, schaap/lam, paard en geit, zoals biefstuk, varkenslapje of gehakt?",
							Order = 33,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 11.1B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties rood vlees eet u op zo’n dag?",
							Description = "(Een normale portie is 100 gram).",
							Order = 34,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						//TODO: 11B
						new() // 11.2A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld bewerkt vlees?",
							Description = "Bewerkt vlees is bijvoorbeeld bacon, hamburger of worst. Alle in de winkel verkrijgbare gekruide/gerookte vleesproducten vallen onder bewerkt vlees.",
							Order = 35,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 11.2B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties bewerkt vlees eet u op zo’n dag?",
							Description = "(Een normale portie is 100 gram).",
							Order = 36,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 12A
						{
							Type = QuestionType.Choice,
							Text = "Op gemiddeld hoeveel dagen per week drinkt u dranken met suiker?",
							Description = "koffie of thee met suiker, zuiveldranken met suiker zoals chocolademelk en yoghurtdrank met suiker, vruchtensappen, energie- of sportdranken, frisdranken zoals cola en sinas, limonade of ranja.",
							Order = 37,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 12B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel glazen drinkt u op zo’n dag?",
							Description = "Een glas is 225mL",
							Order = 38,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 },
								new () { Name = "7", Value = "7", Order = 6 },
								new () { Name = "Meer", Value = "10", Order = 7 }
							}
						},
						new() // 13A
						{
							Type = QuestionType.Choice,
							Text = "Op gemiddeld hoeveel dagen per week drinkt u alcohol?",
							Order = 39,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 13B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel glazen drinkt u op zo’n dag?",
							Order = 40,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 },
								new () { Name = "7", Value = "7", Order = 6 },
								new () { Name = "Meer", Value = "10", Order = 7 }
							}
						},
						new() // 14.1A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld chocola?",
							Description = "(bonbons, chocolaatjes), candybars (mars, snickers etc).",
							Order = 41,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 14.1B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties/stuks chocola eet u op zo’n dag?",
							Order = 42,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 14.2A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld snoep?",
							Description = "(zuurtjes, drop etc).",
							Order = 43,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 14.2B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel stuks snoep eet u op zo’n dag?",
							Order = 44,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 14.3A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld koek, cake, taart of biscuit?",
							Description = "(evergreen, sultana etc).",
							Order = 45,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 14.3B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel stuks koek, cake, taart of biscuit eet u op zo’n dag?",
							Order = 46,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						},
						new() // 14.4A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld hartige tussendoortjes?",
							Description = "Chips, zoute nootjes, kroket, stukjes kaas, worst etc.",
							Order = 47,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Nooit", Value = "0", Order = 0 },
								new () { Name = "1", Value = "1", Order = 1 },
								new () { Name = "2", Value = "2", Order = 2 },
								new () { Name = "3", Value = "3", Order = 3 },
								new () { Name = "4", Value = "4", Order = 4 },
								new () { Name = "5", Value = "5", Order = 5 },
								new () { Name = "6", Value = "6", Order = 6 },
								new () { Name = "Elke dag", Value = "7", Order = 7 }
							}
						},
						new() // 14.4B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties/stuks hartige tussendoortjes eet u op zo’n dag?",
							Order = 48,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "1", Value = "1", Order = 0 },
								new () { Name = "2", Value = "2", Order = 1 },
								new () { Name = "3", Value = "3", Order = 2 },
								new () { Name = "4", Value = "4", Order = 3 },
								new () { Name = "5", Value = "5", Order = 4 },
								new () { Name = "6", Value = "6", Order = 5 }
							}
						}
					}
				},
				Themes = new List<Theme>
				{
					new()
					{
						Id = new Guid("7e1e494e-4dad-4c0f-b448-128552f7869f"),
						Name = "Groenten",
						Description = "Dagelijks ten minste 200 gram groente",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "groenten"
						},
						Unit = ThemeUnit.Grams,
						FrequencyType = FrequencyType.Amount,
						FrequencyGoal = 14,
						CurrentFrequencyQuestion = "Hoe vaak in de week eet je al groente?",
						GoalFrequencyQuestion = "Hoe vaak in de week wil je groente eten?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Groenten",
								Description = "Een portie groente is 100 gram.",
								Image = new Image
								{
									Extension = "png",
									Name = "groenten"
								}
							}
						}
					},
					new()
					{
						Name = "Peulvruchten",
						Description = "Dagelijks ten minste 2 stuks peulvruchten",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "fruit"
						},
						Unit = ThemeUnit.Amount,
						FrequencyType = FrequencyType.Amount,
						FrequencyGoal = 14,
						CurrentFrequencyQuestion = "Hoe vaak in de week eet je al peulvruchten?",
						GoalFrequencyQuestion = "Hoe vaak in de week wil je peulvruchten eten?",
						Faq = new List<Faq>
						{
							new()
							{
								Question = "Wat zijn peulvruchten?",
								Answer = "Peulvruchten zijn bijvoorbeeld bonen, erwten, linzen, kikkererwten en sojabonen.",
							},
							new()
							{
								Question = "Hoeveel peulvruchten moet ik eten?",
								Answer =
									"Een portie peulvruchten is 200 gram. Dat is ongeveer 4 opscheplepels. Een portie peulvruchten is dus 4 keer zoveel als een portie groente.",
							},
						},
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Peulvruchten",
								Description = "Een portie peulvruchten is 100 gram.",
								Image = new Image
								{
									Extension = "png",
									Name = "fruit"
								}

							}
						},
						Recipes = new List<Recipe>
						{
							new()
							{
								Name = "Pasta met peulvruchten",
								Image = new Image
								{
									Extension = "png",
									Name = "fc5fcc51-e19c-4e14-8647-82cff962ba83"
								},
								Steps = new()
								{
									new()
									{
										Description = "Zet water op het vuur",
										Order = 0
									},
									new()
									{
										Description = "Laat het water koken",
										Order = 1
									},
									new()
									{
										Description = "Doe de paste in het water",
										Order = 2
									}
								},
								Ingredients = new()
								{
									new()
									{
										Ingredient = "500ml water"
									},
									new()
									{
										Ingredient = "100 Gram pasta"
									}
								}
							},
							new()
							{
								Name = "Vis met peulvruchten",
								Image = new Image
								{
									Extension = "png",
									Name = "a25515a3-9383-4f44-9597-488ba6393a14"
								},
								Steps = new()
								{
									new()
									{
										Description = "Zet water op het vuur",
										Order = 0
									},
									new()
									{
										Description = "Laat het water koken",
										Order = 1
									},
									new()
									{
										Description = "Doe de paste in het water",
										Order = 2
									}
								},
								Ingredients = new()
								{
									new()
									{
										Ingredient = "500ml water"
									},
									new()
									{
										Ingredient = "100 Gram pasta"
									}
								}
							}
						}
					},
					new()
					{
						Name = "Vis",
						Description = "Eet elke week een keer vis",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "vis"
						},
						Unit = ThemeUnit.Amount,
						FrequencyType = FrequencyType.Amount,
						FrequencyGoal = 1,
						CurrentFrequencyQuestion = "Hoe vaak in de week eet je al vis?",
						GoalFrequencyQuestion = "Hoe vaak in de week wil je vis eten?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Vis",
								Description = "Een portie vis is 100 gram.",
								Image = new Image
								{
									Extension = "png",
									Name = "vis"
								}
							}
						}
					},
					new()
					{
						Name = "Alcohol",
						Description = "Drink minder alcohol",
						Type = ThemeType.Negative,
						Image = new Image
						{
							Extension = "png",
							Name = "dansen"
						},
						Unit = ThemeUnit.Glasses,
						FrequencyType = FrequencyType.Amount,
						FrequencyGoal = 0,
						CurrentFrequencyQuestion = "Hoeveel glazen alcohol drink je per week?",
						GoalFrequencyQuestion = "Hoeveel glazen alcohol wil je maximaal drinken per week?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Alcohol",
								Description = "Minder alcohol drinken",
								Image = new Image
								{
									Extension = "png",
									Name = "geen"
								}
							}
						}
					}
				}
			});

		if (await context.Categories.FindAsync(BewegenId) is null)
			categories.Add(new Category
			{
				Id = BewegenId,
				IsActive = true,
				Name = "Bewegen",
				Color = "3DA9DE",
				Questionnaire = new Questionnaire
				{
					Title = "Intake",
					Description = "Vragen over beweging",
					Questions = new List<Question>
					{
						new() // 0
						{
							Type = QuestionType.Display,
							Text = "Introductie",
							Description = "Neem in uw gedachten een normale week in de afgelopen maanden. Wilt u aangeven hoeveel dagen per week u de onderstaande activiteiten verrichte, hoeveel minuten u daar dan gemiddeld op zo’n dag mee bezig was en hoe inspannend deze activiteit was?",
							Order = 0,
							IsRequired = false
						},
						new() // 1
						{
							Id = default,
							Type = QuestionType.Display,
							Text = "Woon-(vrijwilligers)werk verkeer (heen en terug)",
							Description = "Het gaat hierbij om de activiteit (lopen, fietsen) om naar terugkerende, geplande activiteiten te gaan. Zoals bijvoorbeeld (vrijwilligers)werk, mantel-zorg, oppassen, het volgen van een cursus volgen.",
							Information = "Voorbeeld 1: U fietst 1 dag per week 15 min heen, en 15 min terug om op uw kleinkinderen te passen. U vult dan in 1 dag, totaal 30 minuten fietsen)\r\nVoorbeeld 2: U helpt vrijwillig 3 keer in de week in het buurthuis en loopt hiervoor 10 minuten van huis naar de bus, en op de terug weg loopt u weer 10 minuten van de bus naar huis. U vult dan in 3 dagen in de week, 20 minuten lopen. ",
							Order = 1,
							IsRequired = false
						},
						new() // 1A
						{
							Id = new Guid("12f3f26f-d826-47b6-b208-98ebeab937c1"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week lopen van/naar deze activiteit",
							Order = 2,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 1B
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag lopen van/naar deze activiteit",
							Order = 3,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("12f3f26f-d826-47b6-b208-98ebeab937c1"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 1C
						{
							Type = QuestionType.Choice,
							Text = "Inspanning bij lopen van/naar deze activiteit",
							Order = 4,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("12f3f26f-d826-47b6-b208-98ebeab937c1"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Langzaam", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Snel", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 1D
						{
							Id = new Guid("23a3e071-59e3-4d40-86a4-3fcbe1fb96a8"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week fietsen van/naar deze activiteit",
							Order = 5,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 1E
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag fietsen van/naar deze activiteit",
							Order = 6,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("12f3f26f-d826-47b6-b208-98ebeab937c1"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 1F
						{
							Type = QuestionType.Choice,
							Text = "Inspanning bij fietsen van/naar deze activiteit",
							Order = 7,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("23a3e071-59e3-4d40-86a4-3fcbe1fb96a8"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Langzaam", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Snel", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 2
						{
							Type = QuestionType.Display,
							Text = "Lichamelijke activiteit tijdens (vrijwilligers)werk (niet zijnde huishoudelijk activiteiten en vrijetijdsbestedingen)",
							Description = "Het gaat hierbij om fysieke activiteit tijdens terugkerende, geplande activiteiten te gaan. Zoals bijvoorbeeld (vrijwilligers)werk, mantel-zorg, oppassen, het volgen van een cursus volgen.\r\n\r\nLet op: tel alle activiteiten bij elkaar op.",
							Information = "Voorbeeld 1: Tijdens de 4 uur oppassen op uw kleinkinderen speelt u af en toe actief met ze buiten en bent u zittend of staan met hen bezig. U kunt nu invullen 4 uur licht en matig inspannende activiteit. \r\nVoorbeeld 2: Tijdens de in totaal 6 uur per week (vrijwilligers)werk in het buurthuis bent u ongeveer de helft van de tijd druk bezig met spullen klaar zetten, sjouwen en schoonmaken. De andere helft van de tijd staat u achter de bar. U vult dan bij licht en matig inspannende activiteit 3 uur in, en bij zwaar inspannende activiteit ook drie uur.",
							Order = 8,
							IsRequired = false
						},
						new() // 2A
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per week licht en matig inspannende activiteit",
							Description = "(zittend/staand, met af en toe lopen, zoals bureauwerk of lopend met lichte lasten)",
							Order = 9,
							IsRequired = true
						},
						new() // 2B
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per week zwaar inspannende activiteit",
							Description = "(lopend, waarbij regelmatig zware dingen moeten worden opgetild)",
							Order = 10,
							IsRequired = true
						},
						new() // 3
						{
							Type = QuestionType.Display,
							Text = "Huishoudelijke activiteiten",
							Description = null,
							Order = 11,
							IsRequired = false,
						},
						new() // 3A
						{
							Id = new Guid("acf89dc4-6f24-4db1-8bb8-63ae2d06eca3"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week licht en matig inspannend huishoudelijk werk",
							Description = "(staand werk, zoals koken, afwassen, strijken, kind eten geven/in bad doen en lopen werk, zoals stofzuigen, boodschappen doen)",
							Order = 12,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3B
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag licht en matig inspannend huishoudelijk werk",
							Order = 13,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("acf89dc4-6f24-4db1-8bb8-63ae2d06eca3"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3C
						{
							Id = new Guid("f975c685-2616-4736-ae77-a38b50021f42"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week zwaar inspannend huishoudelijk werk",
							Description = "(vloer schrobben, tapijt uitkloppen, met zware boodschappen lopen)",
							Order = 14,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3D
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag zwaar inspannend huishoudelijk werk ",
							Order = 15,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("f975c685-2616-4736-ae77-a38b50021f42"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3
						{
							Type = QuestionType.Display,
							Text = "Vrije Tijd",
							Description = "Activiteiten voor eigen plezier.",
							Order = 16,
							IsRequired = false
						},
						new() // 3A
						{
							Id = new Guid("dfb09925-e9d4-4e8f-9ab3-97e8d27e1939"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week wandelen",
							Order = 17,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3B
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag wandelen",
							Order = 18,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("dfb09925-e9d4-4e8f-9ab3-97e8d27e1939"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3C
						{
							Type = QuestionType.Choice,
							Text = "Inspanning tijdens wandelen",
							Order = 19,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("dfb09925-e9d4-4e8f-9ab3-97e8d27e1939"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Langzaam", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Snel", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 3D
						{
							Id = new Guid("eb05db70-8725-4778-961e-6f58050f2c3e"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week fietsen",
							Order = 20,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3E
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag fietsen",
							Order = 21,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("eb05db70-8725-4778-961e-6f58050f2c3e"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3F
						{
							Type = QuestionType.Choice,
							Text = "Inspanning tijdens fietsen",
							Order = 22,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("eb05db70-8725-4778-961e-6f58050f2c3e"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Langzaam", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Snel", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 3G
						{
							Id = new Guid("1ade4290-1dfe-4ecc-806a-4f5e04e92f79"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week tuinieren",
							Order = 23,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3H
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag tuinieren",
							Order = 24,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("1ade4290-1dfe-4ecc-806a-4f5e04e92f79"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3I
						{
							Type = QuestionType.Choice,
							Text = "Inspanning tijdens tuinieren",
							Order = 25,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("1ade4290-1dfe-4ecc-806a-4f5e04e92f79"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Licht", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Zwaar", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 3J
						{
							Id = new Guid("2a1ff093-d640-42df-9a3a-4652834b5cf3"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week klussen/doe-het-zelven",
							Order = 26,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3K
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag klussen/doe-het-zelven",
							Order = 27,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("2a1ff093-d640-42df-9a3a-4652834b5cf3"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3L
						{
							Type = QuestionType.Choice,
							Text = "Inspanning tijdens klussen/doe-het-zelven",
							Order = 28,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("2a1ff093-d640-42df-9a3a-4652834b5cf3"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Licht", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Zwaar", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 3M
						{
							Id = new Guid("498fc817-bfe7-461e-a9aa-2bc58b641034"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week balansoefeningen",
							Order = 29,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3N
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag balansoefeningen",
							Order = 30,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("498fc817-bfe7-461e-a9aa-2bc58b641034"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3O
						{
							Type = QuestionType.Choice,
							Text = "Inspanning tijdens balansoefeningen",
							Order = 31,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("498fc817-bfe7-461e-a9aa-2bc58b641034"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Licht", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Zwaar", Value = "3", Order = 2 }
							},
							IsRequired = true
						},
						new() // 3P
						{
							Id = new Guid("68f74c5b-5db4-4706-ab7c-010e4e43b480"),
							Type = QuestionType.Integer,
							Text = "Aantal dagen per week spierversterkende oefeningen",
							Order = 32,
							IsRequired = true,
							Min = 0,
							Max = 7
						},
						new() // 3Q
						{
							Type = QuestionType.Duration,
							Text = "Gemiddelde tijd per dag spierversterkende oefeningen",
							Order = 33,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("68f74c5b-5db4-4706-ab7c-010e4e43b480"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							IsRequired = true
						},
						new() // 3R
						{
							Type = QuestionType.Choice,
							Text = "Inspanning tijdens spierversterkende oefeningen",
							Order = 34,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = new Guid("68f74c5b-5db4-4706-ab7c-010e4e43b480"),
								Operator = Operator.GreaterThan,
								Answer = "0"
							},
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Licht", Value = "1", Order = 0 },
								new() { Name = "Gemiddeld", Value = "2", Order = 1 },
								new() { Name = "Zwaar", Value = "3", Order = 2 }
							},
							IsRequired = true
						}
					}
				},
				Themes = new List<Theme>
				{
					new()
					{
						Name = "Balans",
						Description = "Aanbeveling: Minimaal twee keer per week",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "bewegen"
						},
						Unit = ThemeUnit.Amount,
						FrequencyType = FrequencyType.Amount,
						CurrentActivityQuestion = "Welke beweging doe je al?",
						GoalActivityQuestion = "Welke beweging wil je nog meer doen?",
						CurrentFrequencyQuestion = "Hoe vaak per week doe je dit al?",
						GoalFrequencyQuestion = "Hoe vaak per week wil je dit nog meer doen?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Fietsen",
								Description = "Minuten fietsen",
								Image = new Image
								{
									Extension = "png",
									Name = "fietsen"
								}
							},
							new()
							{
								Name = "Wandelen",
								Description = "Minuten wandelen",
								Image = new Image
								{
									Extension = "png",
									Name = "wandelen"
								}
							},
							new()
							{
								Name = "Gymnastiek",
								Description = "Minuten gymnastiek",
								Image = new Image
								{
									Extension = "png",
									Name = "gymnastiek"
								}
							},
							new()
							{
								Name = "Zwemmen",
								Description = "Minuten zwemmen",
								Image = new Image
								{
									Extension = "png",
									Name = "zwemmen"
								}
							},
							new()
							{
								Name = "Badminton",
								Description = "Minuten badminton",
								Image = new Image
								{
									Extension = "png",
									Name = "badminton"
								}
							}
						}
					},
					new()
					{
						Name = "Bewegen",
						Description = "Aanbeveling: Minimaal 150 minuten per week bewegen",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "bewegen"
						},
						Unit = ThemeUnit.Minutes,
						FrequencyType = FrequencyType.Minutes,
						FrequencyGoal = 150,
						CurrentActivityQuestion = "Welke beweging doe je al?",
						GoalActivityQuestion = "Welke beweging wil je nog meer doen?",
						CurrentFrequencyQuestion = "Hoe veel minuten per week doe je dit al?",
						GoalFrequencyQuestion = "Hoe veel minuten in de week?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Fietsen",
								Description = "Minuten fietsen",
								Image = new Image
								{
									Extension = "png",
									Name = "fietsen"
								},
								Videos = new List<Video>
								{
									new()
									{
										Url = "https://www.youtube.com/watch?v=7VRLk9zUc4Q&t=1s"
									},
									new()
									{
										Url = "https://www.youtube.com/watch?v=GN9Hhv-3o98"
									}
								}
							},
							new()
							{
								Name = "Wandelen",
								Description = "Minuten wandelen",
								Image = new Image
								{
									Extension = "png",
									Name = "wandelen"
								}
							},
							new()
							{
								Name = "Gymnastiek",
								Description = "Minuten gymnastiek",
								Image = new Image
								{
									Extension = "png",
									Name = "gymnastiek"
								}
							},
							new()
							{
								Name = "Zwemmen",
								Description = "Minuten zwemmen",
								Image = new Image
								{
									Extension = "png",
									Name = "zwemmen"
								}
							},
							new()
							{
								Name = "Badminton",
								Description = "Minuten badminton",
								Image = new Image
								{
									Extension = "png",
									Name = "badminton"
								}
							}
						}
					},
					new()
					{
						Name = "Spierkracht",
						Description = "Aanbeveling: Minimaal twee keer per week",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "spierkracht"
						},
						Unit = ThemeUnit.Amount,
						FrequencyType = FrequencyType.Amount,
						CurrentActivityQuestion = "Welke beweging doe je al?",
						GoalActivityQuestion = "Welke beweging wil je nog meer doen?",
						CurrentFrequencyQuestion = "Hoe vaak per week doe je dit al?",
						GoalFrequencyQuestion = "Hoe vaak per week wil je dit nog meer doen?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Fietsen",
								Description = "Minuten fietsen",
								Image = new Image
								{
									Extension = "png",
									Name = "fietsen"
								}
							},
							new()
							{
								Name = "Wandelen",
								Description = "Minuten wandelen",
								Image = new Image
								{
									Extension = "png",
									Name = "wandelen"
								}
							},
							new()
							{
								Name = "Gymnastiek",
								Description = "Minuten gymnastiek",
								Image = new Image
								{
									Extension = "png",
									Name = "gymnastiek"
								}
							},
							new()
							{
								Name = "Zwemmen",
								Description = "Minuten zwemmen",
								Image = new Image
								{
									Extension = "png",
									Name = "zwemmen"
								}
							},
							new()
							{
								Name = "Badminton",
								Description = "Minuten badminton",
								Image = new Image
								{
									Extension = "png",
									Name = "badminton"
								}
							}
						}
					},
					new()
					{
						Name = "Botsterkte",
						Description = "Aanbeveling: Minimaal twee keer per week",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "botsterkte"
						},
						Unit = ThemeUnit.Amount,
						FrequencyType = FrequencyType.Amount,
						CurrentActivityQuestion = "Welke beweging doe je al?",
						GoalActivityQuestion = "Welke beweging wil je nog meer doen?",
						CurrentFrequencyQuestion = "Hoe vaak per week doe je dit al?",
						GoalFrequencyQuestion = "Hoe vaak per week wil je dit nog meer doen?",
						Activities = new List<Activity>
						{
							new()
							{
								Name = "Botsterkte",
								Image = new Image
								{
									Extension = "png",
									Name = "botsterkte"
								}
							}
						}
					}
				}
			});
		
		if (categories.Count > 0)
		{
			context.Categories.AddRange(categories);

			// if (fhirOptions.Write)
			// {
			// 	// Create fhir value set containing all categories
			// 	await categoryDao.Initialize(categories);
			//
			// 	foreach (var category in categories)
			// 	{
			// 		// Add category questionnaire to FHIR database
			// 		await questionnaireDao.Insert(category.Questionnaire);
			// 		
			// 		foreach (var theme in category.Themes)
			// 		{
			// 			// Add theme to FHIR database
			// 			await themeDao.Insert(theme);
			// 		}
			// 	}
			// }

			await context.SaveChangesAsync();
		}
	}
}
