using Microsoft.EntityFrameworkCore;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Activity> Activities { get; }

	DbSet<AnswerOption> AnswerOptions { get; }

	DbSet<CarePlan> CarePlans { get; }

	DbSet<Category> Categories { get; }

	DbSet<Client> Clients { get; }

	DbSet<Coach> Coaches { get; }

	DbSet<Content> Content { get; }

	DbSet<Execution> Executions { get; }

	DbSet<Faq> Faq { get; }

	DbSet<Goal> Goals { get; }

	DbSet<Image> Images { get; }

	DbSet<MotivationalMessage> MotivationalMessages { get; }

	DbSet<Participant> Participants { get; }

	DbSet<Question> Questions { get; }

	DbSet<Questionnaire> Questionnaires { get; }

	DbSet<QuestionnaireResponse> QuestionnaireResponses { get; }

	DbSet<QuestionResponse> QuestionResponses { get; }

	DbSet<Recipe> Recipes { get; }

	DbSet<RecipeIngredient> RecipeIngredients { get; }

	DbSet<RecipeStep> RecipeSteps { get; }

	DbSet<RecommendationRule> RecommendationRules { get; }

	DbSet<RefreshToken> RefreshTokens { get; }

	DbSet<Theme> Themes { get; }

	DbSet<TinyHabit> TinyHabit { get; }

	DbSet<Video> Videos { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
