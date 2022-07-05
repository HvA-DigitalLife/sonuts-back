using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Persistence.Interceptors;

namespace Sonuts.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
{
	private readonly IMediator _mediator;
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IMediator mediator,
		AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		_mediator = mediator;
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
	}

	public DbSet<Activity> Activities => Set<Activity>();

	public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
	public DbSet<CarePlan> CarePlans => Set<CarePlan>();

	public DbSet<Category> Categories => Set<Category>();

	public DbSet<Client> Clients => Set<Client>();

	public DbSet<Coach> Coaches => Set<Coach>();

	public DbSet<Content> Content => Set<Content>();

	public DbSet<Execution> Executions => Set<Execution>();

	public DbSet<Goal> Goals => Set<Goal>();

	public DbSet<Image> Images => Set<Image>();
	
	public DbSet<Participant> Participants => Set<Participant>();

	public DbSet<Question> Questions => Set<Question>();

	public DbSet<Questionnaire> Questionnaires => Set<Questionnaire>();

	public DbSet<QuestionnaireResponse> QuestionnaireResponses => Set<QuestionnaireResponse>();

	public DbSet<QuestionResponse> QuestionResponses => Set<QuestionResponse>();

	public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

	public DbSet<Theme> Themes => Set<Theme>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<IdentityRole>().ToTable("Roles");
		builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
		builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
		builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
		builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
		builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _mediator.DispatchDomainEvents(this);

		return await base.SaveChangesAsync(cancellationToken);
	}
}
