using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

		var ruleQuestionIds = ruleArray.SelectMany(ra => ra.Questions.Select(q => q.Id));

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

		foreach (var rule in ruleArray)
		{
			switch (rule.Type)
			{
				case RecommendationRuleType.All:
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (questionResponses.Any(response => response.Answer != rule.Value)) return false;
							break;
						case Operator.NotEquals:
							if (questionResponses.Any(response => response.Answer == rule.Value)) return false;
							break;
						case Operator.GreaterThan:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) > int.Parse(rule.Value))) return false;
							break;
						case Operator.LessThan:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) < int.Parse(rule.Value))) return false;
							break;
						case Operator.GreaterOrEquals:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) >= int.Parse(rule.Value))) return false;
							break;
						case Operator.LessOrEquals:
							if (questionResponses.All(response => (int.TryParse(response.Answer, out var answer) ? answer : 0) <= int.Parse(rule.Value))) return false;
							break;
						default:
							return false;
					}
					break;
				case RecommendationRuleType.Sum:
					var sum = questionResponses.Sum(response => int.TryParse(response.Answer, out var answer) ? answer : 0);
					var value = int.Parse(rule.Value);
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (sum != value) return false;
							break;
						case Operator.NotEquals:
							if (sum == value) return false;
							break;
						case Operator.GreaterThan:
							if (sum <= value) return false;
							break;
						case Operator.LessThan:
							if (sum >= value) return false;
							break;
						case Operator.GreaterOrEquals:
							if (sum < value) return false;
							break;
						case Operator.LessOrEquals:
							if (sum > value) return false;
							break;
						default:
							return false;
					}
					break;
				case RecommendationRuleType.Any:
					return false; //TODO
				default:
					return false;
			}
		}

		return true;
	}
}
