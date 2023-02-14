using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal static class CategorySeed
{
	private static readonly Guid IntakeId = new("791dac69-f1af-47e2-8d4d-e89a83e54e72");
	private static readonly Guid VoedingId = new("a5997737-7f28-4f5f-92fc-6054023b1248");
	private static readonly Guid BewegenId = new("33c34b68-0925-4af4-a612-11503c87208f");

	internal static async Task Seed(IApplicationDbContext context, IFhirOptions fhirOptions, ICategoryDao categoryDao, IQuestionnaireDao questionnaireDao, IThemeDao themeDao, IActivityDao activityDao)
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
							Order = 1
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw lengte?",
							Description = "In centimeters",
							Order = 2
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Wat is uw gewicht?",
							Description = "In kilogram",
							Order = 3
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
						//TODO: 6.
						new() // 7A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld ongezouten pinda’s of noten?",
							Description = "100% pindakaas of notenspread mag u hierbij meerekenen.",
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
						new() // 7B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel handjes noten of pinda’s eet u op zo’n dag?",
							Description = "kleine  handjes (circa 15 gram) per dag.",
							Order = 10,
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
						//TODO: 8
						new() // 9A
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld magere vis?",
							Description = "Magere vis is bijvoorbeeld forel, kabeljauw, koolvis, pangasius, schelvis, schol, tilapia, tong, tonijn of wijting.",
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
						new() // 9B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties magere vis eet u gemiddeld op zo’n dag?",
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
						new() // 9C
						{
							Type = QuestionType.Choice,
							Text = "Hoe vaak per week eet u gemiddeld vette vis?",
							Description = "Vette vis is bijvoorbeeld bokking, haring, heilbot, makreel, paling, sardines, sprot filet of zalm.",
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
						new() // 9D
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel porties vette vis eet u gemiddeld op zo’n dag?",
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
						new() // 10A
						{
							Type = QuestionType.Choice,
							Text = "Met welke soorten boter besmeert u meestal uw brood, knäckebröd, cracker of beschuit?",
							Order = 15,
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
							Order = 16,
							AnswerOptions = new List<AnswerOption>
							{
								new () { Name = "Ik gebruik meestal geen boter, margarine, olie of andere bakproducten", Value = "Ik gebruik meestal geen boter, margarine, olie of andere bakproducten", Order = 0 },
								new () { Name = "Meestal roomboter", Value = "Meestal roomboter", Order = 1 },
								new () { Name = "Meestal margarine of bakproduct uit een pakje", Value = "Meestal margarine of bakproduct uit een pakje", Order = 2 },
								new () { Name = "Meestal margarine of bakproduct uit een fles", Value = "Meestal margarine of bakproduct uit een fles", Order = 3 },
								new () { Name = "Olie", Value = "Olie", Order = 4 }
							}
						},
						//TODO: 11A
						//TODO: 11B
						new() // 12A
						{
							Type = QuestionType.Choice,
							Text = "Op gemiddeld hoeveel dagen per week drinkt u dranken met suiker?",
							Description = "koffie of thee met suiker, zuiveldranken met suiker zoals chocolademelk en yoghurtdrank met suiker, vruchtensappen, energie- of sportdranken, frisdranken zoals cola en sinas, limonade of ranja.",
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
						new() // 12B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel glazen drinkt u op zo’n dag?",
							Description = "Een glas is 225mL",
							Order = 18,
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
						new() // 13B
						{
							Type = QuestionType.Choice,
							Text = "Hoeveel glazen drinkt u op zo’n dag?",
							Order = 20,
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
						}
						//TODO: 14
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
								DependentQuestionId = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
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
								DependentQuestionId = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
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
								new() { Name = "Langzaam", Value = "Langzaam", Order = 0 },
								new() { Name = "Gemiddeld", Value = "Gemiddeld", Order = 1 },
								new() { Name = "Snel", Value = "Snel", Order = 2 }
							},
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = Guid.Parse("261e4fab-2094-42af-8a6f-808b9a7506cd"),
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
								DependentQuestionId = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
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
								DependentQuestionId = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
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
								new() { Name = "Langzaam", Value = "Langzaam", Order = 0 },
								new() { Name = "Gemiddeld", Value = "Gemiddeld", Order = 1 },
								new() { Name = "Snel", Value = "Snel", Order = 2 }
							},
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = Guid.Parse("2b35733d-1a23-45e4-9185-90e140219bd6"),
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
								DependentQuestionId = Guid.Parse("b8219b70-fef3-4ad5-a04c-f7a08b7d9631"),
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
								DependentQuestionId = Guid.Parse("514b7d7f-1c8e-482d-b35a-d1d5de2092ad"),
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
								DependentQuestionId = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
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
								DependentQuestionId = Guid.Parse("f38ae8ff-bc85-4440-8f57-65187fe1eec7"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("6165ce04-e468-47d5-bddd-84a4e86894da"),
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
								DependentQuestionId = Guid.Parse("6165ce04-e468-47d5-bddd-84a4e86894da"),
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
								DependentQuestionId = Guid.Parse("6165ce04-e468-47d5-bddd-84a4e86894da"),
								Operator = Operator.Equals,
								Answer = "Yes"
							}
						},
						new()
						{
							Id = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
							Type = QuestionType.MultiChoice,
							Text = "Welk van de onderstaande activiteiten doet u in uw vrije tijd?",
							Description = "Meerdere antwoorden mogelijk",
							Order = 18,
							AnswerOptions = new List<AnswerOption>
							{
								new() { Name = "Wandelen", Value = "Wandelen", Order = 0 },
								new() { Name = "Fietsen", Value = "Fietsen", Order = 1 },
								new() { Name = "Tuinieren", Value = "Tuinieren", Order = 2 },
								new() { Name = "Klussen/Doe het zelven", Value = "Klussen/Doe het zelven", Order = 3 },
								new() { Name = "Geen van deze activiteiten", Value = "Geen van deze activiteiten", Order = 4 }
							}
						},
						new()
						{
							Type = QuestionType.Integer,
							Text = "Hoeveel dagen per week?",
							Order = 19,
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
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
								DependentQuestionId = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
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
								new() { Name = "Langzaam", Value = "Langzaam", Order = 0 },
								new() { Name = "Gemiddeld", Value = "Gemiddeld", Order = 0 },
								new() { Name = "Snel", Value = "Snel", Order = 0 }
							},
							EnableWhen = new EnableWhen
							{
								DependentQuestionId = Guid.Parse("9e06be64-7e6a-49fb-8528-726a3c113526"),
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
						Name = "Balans",
						Description = "Aanbeveling: Minimaal twee keer per week",
						Type = ThemeType.Default,
						Image = new Image
						{
							Extension = "png",
							Name = "bewegen"
						},
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

			// FHIR query			
			if (fhirOptions.Write)
			{
				// Create fhir value set containing all categories
				await categoryDao.Initialize(categories);

				foreach (var category in categories)
				{

					// Add category questionnaire to FHIR database
					await questionnaireDao.Insert(category.Questionnaire);
					
					foreach (var theme in category.Themes)
					{
						// Add activities to FHIR database separately
						foreach (var activity in theme.Activities) {
							await activityDao.Insert(activity);
						}


						// Add theme to FHIR database
						await themeDao.Insert(theme);
					}
				}
			}

			await context.SaveChangesAsync();
		}
	}
}
