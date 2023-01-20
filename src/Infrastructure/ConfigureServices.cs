using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Infrastructure.Fhir.Daos;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Files;
using Sonuts.Infrastructure.Identity;
using Sonuts.Infrastructure.Persistence;
using Sonuts.Infrastructure.Persistence.Interceptors;
using Sonuts.Infrastructure.Services;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir;

namespace Sonuts.Infrastructure;

public static class ConfigureServices
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();

		services.AddDbContext<ApplicationDbContext>(options =>
			{
				if (configuration.GetValue<bool>("Database:SensitiveDataLogging"))
					options.EnableSensitiveDataLogging();

				options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"),
					builder =>
					{
						builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
					});
			}
		);

		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		services.AddScoped<ApplicationDbContextInitialiser>();

		services.AddIdentity<User, IdentityRole>()
			.AddUserManager<UserManager<User>>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.AddTransient<IDateTime, DateTimeService>();
		services.AddTransient<IIdentityService, IdentityService>();
		services.AddTransient<ITokenService, TokenService>();
		services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

		services.AddTransient<IActivityDao, FhirActivityDao>();		
		services.AddTransient<ICarePlanDao, FhirCarePlanDao>();
		services.AddTransient<ICategoryDao, FhirCategoryDao>();
		services.AddTransient<IExecutionDao, FhirExecutionDao>();	
		services.AddTransient<IGoalDao, FhirGoalDao>();
		services.AddTransient<IParticipantDao, FhirParticipantDao>();
		services.AddTransient<IQuestionnaireDao, FhirQuestionnaireDao>();
		services.AddTransient<IQuestionnaireResponseDao, FhirQuestionnaireResponseDao>();
		services.AddTransient<IThemeDao, FhirThemeDao>();

		services.AddHttpClient(HttpClientName.Fhir, httpClient =>
		{
			httpClient.BaseAddress = new Uri(configuration.GetConnectionString("Fhir"));
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Add("Accept", "application/fhir+json");
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mib FHIR client");
		});

		services.AddFhir(options =>
		{
			options.Read = configuration.GetValue<bool>("Fhir:Read");
			options.Write = configuration.GetValue<bool>("Fhir:Write");
		});

		return services;
	}
}
