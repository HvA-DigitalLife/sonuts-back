using System.ComponentModel;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Categories.Models;
using Sonuts.Application.Logic.Themes.Models;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Categories.Queries;

public record GetCategoriesQuery : IRequest<ICollection<CategoriesWithRecommendationsVm>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ICollection<CategoriesWithRecommendationsVm>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetCategoriesQueryHandler(ICurrentUserService currentUserService, IApplicationDbContext context, IMapper mapper)
	{
		_currentUserService = currentUserService;
		_context = context;
		_mapper = mapper;
	}

	public async Task<ICollection<CategoriesWithRecommendationsVm>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
			var userId = new Guid(_currentUserService.AuthorizedUserId);

			var categories = await _context.Categories
				.Include(category => category.Themes).ThenInclude(theme => theme.Image)
				.Include(category => category.Themes).ThenInclude(theme => theme.RecommendationRules).ThenInclude(rule => rule.Questions)
				.Where(category => category.IsActive)
				.ToListAsync(cancellationToken);

			var response = new List<CategoriesWithRecommendationsVm>();

			foreach (var category in categories)
			{
				var themes = new List<RecommendedThemeVm>();

				foreach (var theme in category.Themes)
				{
					themes.Add(new RecommendedThemeVm
					{
						Id = theme.Id,
						Name = theme.Name,
						Type = theme.Type,
						Description = theme.Description,
						Image = _mapper.Map<ImageDto>(theme.Image),
						Unit = theme.Unit,
						UnitAmount = theme.UnitAmount,
						FrequencyType = theme.FrequencyType,
						FrequencyGoal = theme.FrequencyGoal,
						CurrentFrequencyQuestion = theme.CurrentFrequencyQuestion,
						GoalFrequencyQuestion = theme.GoalFrequencyQuestion,
						CurrentActivityQuestion = theme.CurrentActivityQuestion,
						GoalActivityQuestion = theme.GoalActivityQuestion,
						IsRecommended = await IsRecommendedTheme(userId, theme.RecommendationRules, cancellationToken)
					});
				}

				response.Add(new CategoriesWithRecommendationsVm
				{
					Id = category.Id,
					IsActive = category.IsActive,
					Name = category.Name,
					Color = category.Color,
					Themes = themes
				});
			}

			return response;
	}

	private async Task<bool> IsRecommendedTheme(Guid userId, IEnumerable<RecommendationRule> rules, CancellationToken cancellationToken)
	{
		var ruleArray = rules.ToArray();

		if (!ruleArray.Any())
			return false;

		var ruleQuestionIds = ruleArray
			.SelectMany(ra => ra.Questions.Select(q => q.Id));

		var lastQuestionResponseIds = (await _context.QuestionnaireResponses
			.Include(qr => qr.Questionnaire)
			.Where(qr => qr.Participant.Id == userId)
			.ToArrayAsync(cancellationToken))
			.OrderByDescending(qr => qr.CreatedAt)
			.DistinctBy(qr => qr.Questionnaire.Id)
			.Select(qr => qr.Id);

		var questionResponses = await _context.QuestionResponses
			.Where(qr => lastQuestionResponseIds.Contains(qr.QuestionnaireResponse.Id) && ruleQuestionIds.Contains(qr.Question.Id))
			.ToArrayAsync(cancellationToken);

		//TODO: hier moet het nieuwe type berekening uitgevoerd worden voor de questions die gecombineerd een waarde opleveren.
		//1. Toevoegen aan de RecommendationRuleType, misschien als "CombinedProduct" ?
		//2. Een case toevoegen voor deze RecommendationRuleType.CombinedProduct met alle bijbehorende typen operators
		//3. Je hebt 2 of meer "sets of questions" nodig om de berekening te maken, waarbij je de producenten van de sets moet combineren in een sum
		foreach (var rule in ruleArray)
		{
			int value;
			switch (rule.Type)
			{
				case RecommendationRuleType.All:
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (questionResponses.Any(response => response.Answer != rule.Value))
								return false;
							break;
						case Operator.NotEquals:
							if (questionResponses.Any(response => response.Answer == rule.Value))
								return false;
							break;
						case Operator.GreaterThan:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) > int.Parse(rule.Value)))
								return false;
							break;
						case Operator.LessThan:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) < int.Parse(rule.Value)))
								return false;
							break;
						case Operator.GreaterOrEquals:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) >= int.Parse(rule.Value)))
								return false;
							break;
						case Operator.LessOrEquals:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) <= int.Parse(rule.Value)))
								return false;
							break;
						default:
							return false;
					}
					break;
				case RecommendationRuleType.Sum:
					var sum = questionResponses.Sum(response => int.TryParse(response.Answer, out var answer) ? answer : 0);
					value = int.Parse(rule.Value);
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (sum != value)
								return false;
							break;
						case Operator.NotEquals:
							if (sum == value)
								return false;
							break;
						case Operator.GreaterThan:
							if (sum <= value)
								return false;
							break;
						case Operator.LessThan:
							if (sum >= value)
								return false;
							break;
						case Operator.GreaterOrEquals:
							if (sum < value)
								return false;
							break;
						case Operator.LessOrEquals:
							if (sum > value)
								return false;
							break;
						default:
							return false;
					}
					break;
				case RecommendationRuleType.Product:
					var product = questionResponses.Aggregate(1, (current, next) => current * (int.TryParse(next.Answer, out var answer) ? answer : 1));
					value = int.Parse(rule.Value);
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (product != value)
								return false;
							break;
						case Operator.NotEquals:
							if (product == value)
								return false;
							break;
						case Operator.GreaterThan:
							if (product <= value)
								return false;
							break;
						case Operator.LessThan:
							if (product >= value)
								return false;
							break;
						case Operator.GreaterOrEquals:
							if (product < value)
								return false;
							break;
						case Operator.LessOrEquals:
							if (product > value)
								return false;
							break;
						default:
							return false;
					}
					break;
				case RecommendationRuleType.SumOfProductsCereal:
					if (questionResponses.Any())
					{
						var sumOfProductsWholemealPasta = int.TryParse(questionResponses[0].Answer, out var answer) ? answer : 0 * (int.TryParse(questionResponses[3].Answer, out var answer2) ? answer2 : 0) * 35;
						var sumOfProductsWholemealCereal = int.TryParse(questionResponses[1].Answer, out var answer3) ? answer3 : 0 * (int.TryParse(questionResponses[2].Answer, out var answer4) ? answer4 : 0) * 50;
						var totalSumOfProductsWholemeal = sumOfProductsWholemealPasta + sumOfProductsWholemealCereal;
						//The user should consume a minimum of 630 grams of wholegrain products per week.
						if (totalSumOfProductsWholemeal > 630)
							return false;
					}
					break;
				case RecommendationRuleType.SumOfProductsDairy:
					if (questionResponses.Any())
					{
						var productsQuestionMilk = (int.TryParse(questionResponses[4].Answer, out var answerAverageDaysMilk) ? answerAverageDaysMilk : 0) * (int.TryParse(questionResponses[2].Answer, out var answerAverageGlassesMilk) ? answerAverageGlassesMilk : 0) * 200;
						var productsQuestionChocolateMilk = (int.TryParse(questionResponses[5].Answer, out var answerAverageDaysChocMilk) ? answerAverageDaysChocMilk : 0) * (int.TryParse(questionResponses[6].Answer, out var answerAverageGlassesChocMilk) ? answerAverageGlassesChocMilk : 0) * 200;
						var productQuestionCustard = (int.TryParse(questionResponses[1].Answer, out var answerAverageDaysCustard) ? answerAverageDaysCustard : 0) * (int.TryParse(questionResponses[0].Answer, out var answerAverageBowlsCustard) ? answerAverageBowlsCustard : 0) * 150;
						var productQuestionYoghurt = (int.TryParse(questionResponses[3].Answer, out var answerAverageDaysYoghurt) ? answerAverageDaysYoghurt : 0) * (int.TryParse(questionResponses[7].Answer, out var answerAverageBowlsYoghurt) ? answerAverageBowlsYoghurt : 0) * 150;
						var totalSumOfProductsDairy = productsQuestionMilk + productsQuestionChocolateMilk + productQuestionCustard + productQuestionYoghurt;
						//The user should consume a minimum of 2450 ml of dairy per week.
						if (totalSumOfProductsDairy > 2450)
							return false;
					}
					break;
				case RecommendationRuleType.SumOfProductsMeat:
					if (questionResponses.Any())
					{
						var portionsOfRedMeat = (int.TryParse(questionResponses[1].Answer, out var answerAverageDaysRedMeat) ? answerAverageDaysRedMeat : 0) * (int.TryParse(questionResponses[3].Answer, out var answerPortionsRedMeat) ? answerPortionsRedMeat : 0);
						var portionsOfProcessedMeat = (int.TryParse(questionResponses[0].Answer, out var answerAverageDaysProcessedMeat) ? answerAverageDaysProcessedMeat : 0) * (int.TryParse(questionResponses[2].Answer, out var answerPortionsProcessedMeat) ? answerPortionsProcessedMeat : 0);
						var totalPortionsMeatPerWeek = portionsOfRedMeat + portionsOfProcessedMeat;
						//Three portions of red or processed meat are allowed per week.
						if (totalPortionsMeatPerWeek < 4)
							return false;
					}
					break;
				case RecommendationRuleType.SumOfProductsFish:
					if (questionResponses.Any())
					{
						var portionsOfLeanFish = (int.TryParse(questionResponses[1].Answer, out var answerAverageDaysLeanFish) ? answerAverageDaysLeanFish : 0) * (int.TryParse(questionResponses[2].Answer, out var answerPortionsLeanFish) ? answerPortionsLeanFish : 0);
						var portionsOfOilyFish = (int.TryParse(questionResponses[0].Answer, out var answerAverageDaysOilyFish) ? answerAverageDaysOilyFish : 0) * (int.TryParse(questionResponses[3].Answer, out var answerPortionsOilyFish) ? answerPortionsOilyFish : 0);
						var totalPortionsFishPerWeek = portionsOfLeanFish + portionsOfOilyFish;
						//The user should at least eat 1 portion of fish per week.
						if (totalPortionsFishPerWeek > 1)
							return false;
					}
					break;
				case RecommendationRuleType.SumOfProductsCandy:
					if (questionResponses.Any())
					{
						//Deze gaat mis als niet alle vragen hiervoor zijn ingevuld!!
						var portionsChocolate = (int.TryParse(questionResponses[7].Answer, out var answerAverageDaysChocolate) ? answerAverageDaysChocolate : 0) * (int.TryParse(questionResponses[5].Answer, out var answerPortionsChocolate) ? answerPortionsChocolate : 0);
						var portionsSweets = (int.TryParse(questionResponses[4].Answer, out var answerAverageDaysSweets) ? answerAverageDaysSweets : 0) * (int.TryParse(questionResponses[6].Answer, out var answerPortionsSweets) ? answerPortionsSweets : 0);
						var portionsBiscuits = (int.TryParse(questionResponses[2].Answer, out var answerAverageDaysBiscuits) ? answerAverageDaysBiscuits : 0) * (int.TryParse(questionResponses[3].Answer, out var answerPortionsBiscuits) ? answerPortionsBiscuits : 0);
						var portionsSnacks = (int.TryParse(questionResponses[1].Answer, out var answerAverageDaysSnacks) ? answerAverageDaysSnacks : 0) * (int.TryParse(questionResponses[7].Answer, out var answerPortionsSnacks) ? answerPortionsSnacks : 0);
						var totalPortionsCandyPerWeek = portionsChocolate + portionsSweets + portionsBiscuits + portionsSnacks;
						//The recommendation is to eat no more than 3 snacks each week.
						if (totalPortionsCandyPerWeek < 4)
							return false;
					}
					break;
				case RecommendationRuleType.SumOfProductsExercise:
					if (questionResponses.Any())
					{		
						//commute variable set up
						//5: Average time per day walking to/from this activity
						var answerTimeWalkCommute = questionResponses[5].Answer.Split(":");
						var hoursTimeWalkCommute = int.Parse(answerTimeWalkCommute[0]);
						var minutesTimeWalkCommute = int.Parse(answerTimeWalkCommute[1]);
						var averageTimeOfWalkingCommute = (hoursTimeWalkCommute * 60) + minutesTimeWalkCommute;

						//6: Average time per day cycling to/from this activity
						var answerTimeCycleCommute = questionResponses[6].Answer.Split(":");
						var hoursTimeCycleCommute = int.Parse(answerTimeCycleCommute[0]);
						var minutesTimeCycleCommute = int.Parse(answerTimeCycleCommute[1]);
						var averageTimeOfCyclingCommute = (hoursTimeCycleCommute * 60) + minutesTimeCycleCommute;

						//leisure time variable set up
						//1: Average time per day walking
						var answerTimeWalkLeisure = questionResponses[1].Answer.Split(":");
						var hoursTimeWalkLeisure = int.Parse(answerTimeWalkLeisure[0]);
						var minutesTimeWalkLeisure = int.Parse(answerTimeWalkLeisure[1]);
						var averageTimeOfWalkingLeisure = (hoursTimeWalkLeisure * 60) + minutesTimeWalkLeisure;

						//7: Average time per day cycling
						var answerTimeCycleLeisure = questionResponses[7].Answer.Split(":");
						var hoursTimeCycleLeisure = int.Parse(answerTimeCycleLeisure[0]);
						var minutesTimeCycleLeisure = int.Parse(answerTimeCycleLeisure[1]);
						var averageTimeOfCyclingLeisure = (hoursTimeCycleLeisure * 60) + minutesTimeCycleLeisure;


						//commuting calculations
						// 3 * 5
						//3: Number of days per week walking to/from this activity
						var walkForCommute = (int.TryParse(questionResponses[3].Answer, out var answerAverageDaysWalkingCommute) ? answerAverageDaysWalkingCommute : 0) * averageTimeOfWalkingCommute;
						// 2 * 6
						//2: Number of days per week cycling to/from this activity
						var cycleForCommute = (int.TryParse(questionResponses[2].Answer, out var answerAverageDaysCyclingCommute) ? answerAverageDaysCyclingCommute : 0) * averageTimeOfCyclingCommute;

						//leisure time calculations
						// 4 * 1
						//4: Number of days per week walking
						var walkLeisureTime = (int.TryParse(questionResponses[3].Answer, out var answerAverageDaysWalkingLeisure) ? answerAverageDaysWalkingLeisure : 0) * averageTimeOfWalkingLeisure;
						// 0 * 7
						//0: Number of days per week cycling
						var cycleLeisureTime = (int.TryParse(questionResponses[0].Answer, out var answerAverageDaysCyclingLeisure) ? answerAverageDaysCyclingLeisure : 0) * averageTimeOfCyclingLeisure;
						var totalExercisePerWeek = walkForCommute + cycleForCommute + walkLeisureTime + cycleLeisureTime;
						if (totalExercisePerWeek > 150)
						{
							return false;
						}
					}
					break;
				case RecommendationRuleType.Any:
					//TODO: Fix 'Any' rule combined with other rules returning true before others have been checked
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (questionResponses.Any(response => response.Answer == rule.Value))
								return true;
							break;
						case Operator.NotEquals:
							if (questionResponses.Any(response => response.Answer != rule.Value))
								return true;
							break;
						case Operator.GreaterThan:
							if (questionResponses.Any(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) > int.Parse(rule.Value)))
								return true;
							break;
						case Operator.LessThan:
							if (questionResponses.Any(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) < int.Parse(rule.Value)))
								return true;
							break;
						case Operator.GreaterOrEquals:
							if (questionResponses.Any(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) >= int.Parse(rule.Value)))
								return true;
							break;
						case Operator.LessOrEquals:
							if (questionResponses.Any(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) <= int.Parse(rule.Value)))
								return true;
							break;
						default:
							return false;
					}
					break;
				default:
					return false;
			}
		}

		return true;
	}
}
