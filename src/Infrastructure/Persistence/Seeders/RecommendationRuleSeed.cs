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
			var recommendationRule = new RecommendationRule
			{
				Id = Guid.Parse("fa05caf6-d202-430d-8b32-0561f8113160"),
				Type = RecommendationRuleType.Sum,
				Operator = Operator.LessOrEquals,
				Value = "6",
				Questions = new List<Question>
				{
					await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("c05d9393-328c-4e54-a504-7330445fd8cf"))),
					await context.Questions.FirstAsync(question => question.Id.Equals(new Guid("d14f00d4-32f2-4a22-8829-7d7ed86a127c")))
				}
			};
			context.RecommendationRules.Add(recommendationRule);
			(await context.Themes.FirstAsync(t => t.Id.Equals(new Guid("7e1e494e-4dad-4c0f-b448-128552f7869f")))).RecommendationRules.Add(recommendationRule);

			await context.SaveChangesAsync();
		}
	}
}
