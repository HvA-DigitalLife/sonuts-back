using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Categories.Models;
using Sonuts.Application.Logic.Themes.Models;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Categories.Queries;

public record GetCategoriesQuery : IRequest<ICollection<CategoriesWithRecommendationsVm>>;

internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ICollection<CategoriesWithRecommendationsVm>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IFhirOptions _fhirOptions;
	private readonly ICategoryDao _dao;
	
	public GetCategoriesQueryHandler(ICurrentUserService currentUserService, IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, ICategoryDao dao)
	{
		_currentUserService = currentUserService;
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<ICollection<CategoriesWithRecommendationsVm>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.AuthorizedUserId;

		var categories = _fhirOptions.Read ?  
			await _dao.SelectAll():
			await _context.Categories
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
					Description = theme.Description,
					Image = _mapper.Map<ImageDto>(theme.Image),
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

	private async Task<bool> IsRecommendedTheme(string userId, List<RecommendationRule> rules, CancellationToken cancellationToken)
	{
		foreach (var rule in rules)
		{
			var questionResponses = await _context.QuestionResponses
				.Where(questionResponse =>
					questionResponse.QuestionnaireResponse.Participant.Id.Equals(new Guid(userId)) &&
					rule.Questions
						.Select(question => question.Id)
						.Contains(questionResponse.Question.Id))
				.ToListAsync(cancellationToken);

			if (questionResponses.Count < rule.Questions.Count) //TODO: Check if this be done per operator if not all questions are required
				return false;

			switch (rule.Type)
			{
				case RecommendationRuleType.All:
					switch (rule.Operator)
					{
						case Operator.Equals:
							if (!questionResponses.All(response => response.Answer.Equals(rule.Value))) return false;
							break;
						case Operator.NotEquals:
							if (questionResponses.Any(response => response.Answer.Equals(rule.Value))) return false;
							break;
						case Operator.GreaterThan:
							if (questionResponses.All(response => int.Parse(response.Answer) > int.Parse(rule.Value))) return false;
							break;
						case Operator.LessThan:
							if (questionResponses.All(response => int.Parse(response.Answer) < int.Parse(rule.Value))) return false;
							break;
						case Operator.GreaterOrEquals:
							if (questionResponses.All(response => int.Parse(response.Answer) >= int.Parse(rule.Value))) return false;
							break;
						case Operator.LessOrEquals:
							if (questionResponses.All(response => int.Parse(response.Answer) <= int.Parse(rule.Value))) return false;
							break;
						default:
							return false;
					}
					break;
				case RecommendationRuleType.Sum:
					var sum = questionResponses.Sum(response => int.Parse(response.Answer));
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
				default:
					return false;
			}
		}

		return true;
	}
}
