using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal class RecommendationRuleSeed
{
	internal static async Task Seed(IApplicationDbContext context)
	{
		if (!await context.RecommendationRules.AnyAsync())
		{
			var rules = new List<(Guid ThemeId, RecommendationRule RecommendationRule)>
			{
				new()
				{
					ThemeId = new Guid("7e1e494e-4dad-4c0f-b448-128552f7869f"), // Groenten
					RecommendationRule = new RecommendationRule
					{
						Type = RecommendationRuleType.Sum,
						Operator = Operator.LessThan,
						Value = "28",
						Questions = new List<Question>
						{
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("c05d9393-328c-4e54-a504-7330445fd8cf"))), // 4A
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("d14f00d4-32f2-4a22-8829-7d7ed86a127c"))) // 4B
						}
					}
				},
				new()
				{
					ThemeId = new Guid("8452671a-fa49-4aa7-aed4-cad36bea9553"), // Peulvruchten
					RecommendationRule = new RecommendationRule
					{
						Type = RecommendationRuleType.Sum,
						Operator = Operator.LessThan,
						Value = "5",
						Questions = new List<Question>
						{
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("e9e1f1e4-d666-4936-a52b-50dd3b02f086"))), // 5A
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("3b1be15f-5b3d-4be6-b106-fd586396def3"))) // 5B
						}
					}
				},
				new()
				{
					ThemeId = new Guid("b9db755f-5d6c-4797-9373-d95b0633bbb8"), // Vis
					RecommendationRule = new RecommendationRule
					{
						Type = RecommendationRuleType.Sum,
						Operator = Operator.LessThan,
						Value = "1",
						Questions = new List<Question>
						{
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("5de4555f-4780-4e27-ab5e-24b0358ba6e6"))), // 9A
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("a696cbfa-eae0-4015-938c-31b9f641005d"))), // 9B
						}
					}
				},
				new()
				{
					ThemeId = new Guid("b9db755f-5d6c-4797-9373-d95b0633bbb8"), // Vis
					RecommendationRule = new RecommendationRule
					{
						Type = RecommendationRuleType.Sum,
						Operator = Operator.LessThan,
						Value = "1",
						Questions = new List<Question>
						{
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("b547d936-3b8b-4386-81af-6cac317aa858"))), // 9C
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("e9152e08-4ee9-4d3b-8608-f590d31663be"))) // 9D
						}
					}
				},
				new()
				{
					ThemeId = new Guid("5a06f5c6-b3bd-4a88-a200-e0342e46e50a"), // Alcohol
					RecommendationRule = new RecommendationRule
					{
						Type = RecommendationRuleType.Sum,
						Operator = Operator.GreaterThan,
						Value = "1",
						Questions = new List<Question>
						{
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("48227a6a-1aa2-49ea-9837-0acdae9b8297"))), // 13A
							await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("f06d8859-416c-4050-8de2-1b0a8ed4717f"))) // 13B
						}
					}
				}
			};

			foreach (var rule in rules)
			{
				context.RecommendationRules.Add(rule.RecommendationRule);
				(await context.Themes.FirstAsync(t => t.Id.Equals(rule.ThemeId))).RecommendationRules.Add(rule.RecommendationRule);
			}

			await context.SaveChangesAsync();
		}
	}
}
