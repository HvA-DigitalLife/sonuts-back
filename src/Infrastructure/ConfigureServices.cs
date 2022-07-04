using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Common;
using Sonuts.Infrastructure.Fhir.Daos;
using Sonuts.Infrastructure.Files;
using Sonuts.Infrastructure.Identity;
using Sonuts.Infrastructure.Persistence;
using Sonuts.Infrastructure.Persistence.Interceptors;
using Sonuts.Infrastructure.Services;

namespace Sonuts.Infrastructure;

public static class ConfigureServices
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
		IConfiguration configuration,
		IWebHostEnvironment environment)
	{
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();

		if (configuration.GetValue<bool>("UseInMemoryDatabase") || environment.IsEnvironment("Testing"))
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseInMemoryDatabase($"{environment.ApplicationName}-{environment.EnvironmentName}" ));
		}
		else
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"),
					builder =>
					{
						builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					})
			);
		}

		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		services.AddScoped<ApplicationDbContextInitialiser>();

		services.AddIdentity<User, IdentityRole>()
			.AddUserManager<UserManager<User>>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.AddTransient<IDateTime, DateTimeService>();
		services.AddTransient<IIdentityService, IdentityService>();
		services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
		
		services.AddTransient<IQuestionnaireDao, FhirQuestionnaireDao>();

		services.AddHttpClient(HttpClientName.Fhir, httpClient =>
		{
			httpClient.BaseAddress = new Uri(configuration.GetConnectionString("Fhir"));
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Add("Accept", "application/fhir+json");
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mib FHIR client");
		});

		return services;
	}
}
