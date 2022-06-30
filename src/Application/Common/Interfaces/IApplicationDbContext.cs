using Microsoft.EntityFrameworkCore;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Activity> Activities { get; }

	DbSet<AnswerOption> AnswerOptions { get; }

	DbSet<Category> Categories { get; }

	DbSet<Coach> Coaches { get; }

	DbSet<Domain.Entities.Content> Content { get; }

	DbSet<Execution> Executions { get; }

	DbSet<Image> Images { get; }

	DbSet<Goal> Intentions { get; }

	DbSet<Participant> Participants { get; }

	DbSet<Question> Questions { get; }

	DbSet<Questionnaire> Questionnaires { get; }

	DbSet<QuestionnaireResponse> QuestionnaireResponses { get; }

	DbSet<QuestionResponse> QuestionResponses { get; }

	DbSet<Theme> Themes { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
