using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

// ReSharper disable StringLiteralTypo
namespace Sonuts.Infrastructure.Persistence.Seeders;

internal static class CategorySeed
{
	private static readonly Guid IntakeId = new("791dac69-f1af-47e2-8d4d-e89a83e54e72");
	private static readonly Guid VoedingId = new("a5997737-7f28-4f5f-92fc-6054023b1248");
	private static readonly Guid BewegenId = new("33c34b68-0925-4af4-a612-11503c87208f");

	internal static async Task Seed(IApplicationDbContext context)
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
							Type = QuestionType.Integer,
							Text = "Wat is uw leeftijd?",
							Order = 0,
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Wat is uw geslacht?",
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
							Type = QuestionType.String,
							Text = "In welk land ben u zelf geboren?",
							Order = 2
						},
						new()
						{
							Type = QuestionType.String,
							Text = "In welk land is uw moeder geboren?",
							Order = 3
						},
						new()
						{
							Type = QuestionType.String,
							Text = "In welk land is uw vader geboren?",
							Order = 4
						},
						new()
						{
							Type = QuestionType.OpenChoice,
							Text = "Welke van onderstaande beschrijvingen past het beste bij uw situatie?",
							Description = "(Zet een kruisje in het vakje dat het best bij u past)",
							Order = 5,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Alleenstaand", Order = 0 },
								new() { Value = "Getrouwd", Order = 1 },
								new() { Value = "Samenwonend", Order = 2 },
								new() { Value = "Apart wonend", Order = 3 },
								new() { Value = "Weduwnaar", Order = 4 },
								new() { Value = "Gescheiden", Order = 5 }
							},
							OpenAnswerLabel = "Anders, namelijk"
						},
						new()
						{
							Type = QuestionType.Boolean,
							Text = "Heeft u kinderen?",
							Order = 6
						},
						new()
						{
							Type = QuestionType.OpenChoice,
							Text = "Wat is de hoogste opleiding die u heeft afgerond?",
							Order = 7,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Geen opleiding afgemaakt", Order = 0 },
								new() { Value = "Basisschool of lager beroepsonderwijs (LEAO, LTS)", Order = 1 },
								new() { Value = "Voorbereidend Middelbaar Beroepsonderwijs (VMBO); middelbaar algemeen voortgezet onderwijs (MAVO/MULO)", Order = 3 },
								new() { Value = "Middelbaar beroepsonderwijs (MBO / MTS / MEAO); hoger algemeen voortgezet onderwijs (HAVO); voorbereidend wetenschappelijk onderwijs(VWO)", Order = 4 },
								new() { Value = "Hoger beroepsonderwijs (bijvoorbeeld HBO / HEAO / PABO)", Order = 5 },
								new() { Value = "Wetenschappelijk onderwijs / universiteit", Order = 6 }
							},
							OpenAnswerLabel = "Anders, namelijk"
						},
						new()
						{
							Id = Guid.Parse("e9efcf8e-955d-4017-9ab3-d57f2238cd02"),
							Type = QuestionType.MultiOpenChoice,
							Text = "Wat is uw arbeidspositie?",
							Description = "(U mag een kruisje in meer dan een vakje zetten)",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Werken - voltijds", Order = 0 },
								new() { Value = "Werken - deeltijds", Order = 1 },
								new() { Value = "Gepensioneerd", Order = 3 },
								new() { Value = "Huisman/vrouw", Order = 4 },
								new() { Value = "Student", Order = 5 },
								new() { Value = "Niet werkzaam (maar ook niet gepensioneerd)", Order = 6 },
								new() { Value = "Blijvend arbeidsongeschikt / ziek", Order = 7 }
							},
							OpenAnswerLabel = "Anders, Namelijk"
						},
						new()
						{
							Type = QuestionType.String,
							Text = "Beschrijf uw baan",
							Order = 9,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("e9efcf8e-955d-4017-9ab3-d57f2238cd02"),
								Operator = Operator.Equals,
								Answer = "Werken - voltijds"
							}
						},
						new()
						{
							Type = QuestionType.String,
							Text = "Beschrijf uw baan",
							Order = 10,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("e9efcf8e-955d-4017-9ab3-d57f2238cd02"),
								Operator = Operator.Equals,
								Answer = "Werken - deeltijds"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw lengte",
							Description = "In centimeters",
							Order = 11
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw gewicht?",
							Description = "In kilogram",
							Order = 12
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
						new()
						{
							Type = QuestionType.MultiOpenChoice,
							Text = "Heeft u speciale voedingsgewoontes?",
							Description = "Meerdere antwoorden mogelijk",
							Order = 0,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Nee", Order = 0 },
								new() { Value = "Ik eet vegetarisch (geen vis, geen vlees)", Order = 1 },
								new() { Value = "Ik eet geen vlees, maar wel vis", Order = 2 },
								new() { Value = "Ik eet veganistisch", Order = 3 },
								new() { Value = "Ik eet geen varkensvlees", Order = 4 },
								new() { Value = "Ik eet geen koeienvlees", Order = 5 },
								new() { Value = "Ik eet flexitarisch", Order = 6 }
							},
							OpenAnswerLabel = "Ja, anders namelijk"
						},
						new()
						{
							Id = new Guid("eff4185b-c2ad-4746-8802-7ec3b00985d4"),
							Type = QuestionType.MultiOpenChoice,
							Text = "Volgt u een speciaal dieet?",
							Order = 1,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Glutenvrij", Order = 0 },
								new() { Value = "Lactosebeperkt/lactosevrij", Order = 1 },
								new() { Value = "Zoutbeperkt", Order = 2 },
								new() { Value = "Ik ben allergisch voor 1 of meerdere voedingsmiddelen (e.g. pinda’s, schaaldieren)", Order = 3 },
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
								QuestionId = new Guid("eff4185b-c2ad-4746-8802-7ec3b00985d4"),
								Operator = Operator.Equals,
								Answer = "Ik ben allergisch voor 1 of meerdere voedingsmiddelen (e.g. pinda’s, schaaldieren)"
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Op gemiddeld hoeveel dagen per week eet u drie hoofdmaaltijden?",
							Description = "(ontbijt, lunch, avondmaaltijd)",
							Order = 3,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "Nooit", Order = 0 },
								new () { Value = "1", Order = 1 },
								new () { Value = "2", Order = 2 },
								new () { Value = "3", Order = 3 },
								new () { Value = "4", Order = 4 },
								new () { Value = "5", Order = 5 },
								new () { Value = "6", Order = 6 },
								new () { Value = "Elke dag", Order = 7 }
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u, gemiddeld gezien, fruit?",
							Order = 4,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "Nooit", Order = 0 },
								new () { Value = "1", Order = 1 },
								new () { Value = "2", Order = 2 },
								new () { Value = "3", Order = 3 },
								new () { Value = "4", Order = 4 },
								new () { Value = "5", Order = 5 },
								new () { Value = "6", Order = 6 },
								new () { Value = "Elke dag", Order = 7 }
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties fruit eet u gemiddeld op een dag dat u fruit eet?",
							Description = "Een portie fruit is bijvoorbeeld: een appel, een banaan, twee mandarijntjes, twee kiwi’s, een dessertschaaltje gesneden of klein (bessen, aardbeien) fruit. U mag hierbij vruchtensap niet meerekenen.",
							Order = 5,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "1", Order = 0 },
								new () { Value = "2", Order = 1 },
								new () { Value = "3", Order = 2 },
								new () { Value = "4", Order = 3 },
								new () { Value = "5", Order = 4 },
								new () { Value = "6", Order = 5 },
								new () { Value = "Meer porties per dag", Order = 6 }
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u, gemiddeld gezien, groente of rauwkost?",
							Description = "Zowel verse, diepvries als groente in conservenblik tellen mee.",
							Order = 6,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "Nooit", Order = 0 },
								new () { Value = "1", Order = 1 },
								new () { Value = "2", Order = 2 },
								new () { Value = "3", Order = 3 },
								new () { Value = "4", Order = 4 },
								new () { Value = "5", Order = 5 },
								new () { Value = "6", Order = 6 },
								new () { Value = "Elke dag", Order = 7 }
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel opscheplepels/porties groente of rauwkost (circa 50 gram) eet u gemiddeld op een dag dat u groente eet?",
							Order = 7,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "1", Order = 0 },
								new () { Value = "2", Order = 1 },
								new () { Value = "3", Order = 2 },
								new () { Value = "4", Order = 3 },
								new () { Value = "5", Order = 4 },
								new () { Value = "6", Order = 5 }
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per maand eet u, gemiddeld gezien, peulvruchten?",
							Description = "Peulvruchten zijn bijvoorbeeld bruine bonen, witte bonen, kapucijners, kidney(nier)bonen, linzen of kikkererwten. Reken doperwten, tuinbonen en sperziebonen niet mee, deze moeten bij groente worden ingevuld.",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "Nooit", Order = 0 },
								new () { Value = "Een keer per maand", Order = 1 },
								new () { Value = "2-3 keer per maand", Order = 2 },
								new () { Value = "1 keer per week", Order = 3 },
								new () { Value = "2-3 keer per week", Order = 4 },
								new () { Value = "Meer dan 3 keer per week", Order = 5 }
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel peulvruchten eet u op ’een dag dat u peulvruchten eet?",
							Description = "Opscheplepels (circa 50 gram) per dag (een portie = 200 gram, dus 4 opscheplepels).",
							Order = 8,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "1", Order = 0 },
								new () { Value = "2", Order = 1 },
								new () { Value = "3", Order = 2 },
								new () { Value = "4", Order = 3 },
								new () { Value = "5", Order = 4 },
								new () { Value = "6", Order = 5 }
							}
						},
						//TODO: 6.
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld ongezouten pinda’s of noten?",
							Description = "100% pindakaas of notenspread mag u hierbij meerekenen.",
							Order = 9,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Value = "Nooit", Order = 0 },
								new () { Value = "1", Order = 1 },
								new () { Value = "2", Order = 2 },
								new () { Value = "3", Order = 3 },
								new () { Value = "4", Order = 4 },
								new () { Value = "5", Order = 5 },
								new () { Value = "6", Order = 6 },
								new () { Value = "Elke dag", Order = 7 }
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
					},
					new()
					{
						Name = "Fruib",
						Description = "Dagelijks ten minste 2 stuks fruit",
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
								Description = "Eet fruit bij het avond eten",
								Image = new Image()
							},
							new()
							{
								Name = "Lunch",
								Description = "Eet fruit bij de lunch",
								Image = new Image()
							},
							new()
							{
								Name = "Ontbijt",
								Description = "Eet fruit bij het ontbijt",
								Image = new Image()
							},
							new()
							{
								Name = "Snack",
								Description = "Eet fruit als snack",
								Image = new Image()
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
						new()
						{
							Id = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
							Type = QuestionType.Boolean,
							Text = "Is er sprake van lopen van/naar werk?",
							Order = 0,
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
								Answer = "Yes"
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
								Answer = "Yes"
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoe inspannend was deze activiteit?",
							Order = 3,
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
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
							Type = QuestionType.Boolean,
							Text = "Is er sprake van fietsen van/naar werk?",
							Order = 4,
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel dagen per week?",
							Order = 5,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 6,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Type = QuestionType.Choice,
							Text = "Hoe inspannend was deze activiteit?",
							Order = 7,
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
								QuestionId = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("b8219b70-fef3-4ad5-a04c-f7a08b7d9631"),
							Type = QuestionType.Boolean,
							Text = "Is er sprake van licht en matig inspannend werk?",
							Description = "Zittend/staand werk, met af en toe lopen, zoals bureauwerk of lopend werk met lichte lasten.",
							Order = 8
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 9,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("b8219b70-fef3-4ad5-a04c-f7a08b7d9631"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("514b7d7f-1c8e-482d-b35a-d1d5de2092ad"),
							Type = QuestionType.Boolean,
							Text = "Is er sprake van zwaar inspannend werk?",
							Description = "Lopend werk, waarbij regelmatig zware dingen moeten worden opgetild?",
							Order = 10
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 11,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("514b7d7f-1c8e-482d-b35a-d1d5de2092ad"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
							Type = QuestionType.Boolean,
							Text = "Is er sprake van licht en matig inspannend huishoudelijk werk?",
							Description = "Denk aan staand werk, zoals koken, afwassen, strijken, kind eten geven/in bad doen en lopend werk, zoals stofzuigen, boodschappen doen.",
							Order = 12
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel dagen per week?",
							Order = 13,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 14,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
							Type = QuestionType.Boolean,
							Text = " Is er sprake van zwaar inspannend huishoudelijk werk?",
							Description = "Denk aan vloer schrobben, tapijt uitkloppen, met zware boodschappen lopen.",
							Order = 15
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel dagen per week?",
							Order = 16,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 17,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
							Type = QuestionType.MultiChoice,
							Text = "Welk van de onderstaande activiteiten doet u in uw vrije tijd?",
							Order = 18,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Wandelen", Order = 0 },
								new() { Value = "Fietsen", Order = 1 },
								new() { Value = "Tuinieren", Order = 2 },
								new() { Value = "Klussen/Doe het zelven", Order = 3 },
								new() { Value = "Geen van deze activiteiten", Order = 4 }
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel dagen per week?",
							Order = 19,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
								Operator = Operator.NotEquals,
								Answer = "Geen van deze activiteiten"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel minuten gemiddeld per dag?",
							Order = 20,
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
								Operator = Operator.NotEquals,
								Answer = "Geen van deze activiteiten"
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoe inspannend was deze activiteit?",
							Order = 21,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Value = "Langzaam", Order = 0 },
								new() { Value = "Gemiddeld", Order = 0 },
								new() { Value = "Snel", Order = 0 }
							},
							EnableWhen = new EnableWhen
							{
								QuestionId = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
								Operator = Operator.NotEquals,
								Answer = "Geen van deze activiteiten"
							}
						},
						//TODO: 8
						new()
						{
							Type = QuestionType.Integer,
							Text = " Op gemiddeld hoeveel dagen per week ben u, alles bijelkaar opgeteld, tenminste een halfuur bezig met fietsen, klussen, tuinieren of sporten?",
							Order = 22
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
					},
					new()
					{
						Name = "Crossfit",
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

		if (categories.Count > 0)
		{
			foreach (var category in categories)
			{
				context.Categories.Add(category);
			}

			await context.SaveChangesAsync();
		}
	}
}
