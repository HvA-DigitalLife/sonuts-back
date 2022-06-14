using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sonuts.Application.Common.Interfaces;
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
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
					builder =>
					{
						builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					})
			);
		}

		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		services.AddScoped<ApplicationDbContextInitialiser>();

		services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddUserManager<UserManager<ApplicationUser>>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.AddTransient<IDateTime, DateTimeService>();
		services.AddTransient<IIdentityService, IdentityService>();
		services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
		
		return services;
	}
}
