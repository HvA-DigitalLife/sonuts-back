using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Infrastructure.Persistence;
using Sonuts.Presentation.Common;
using Sonuts.Presentation.Common.Converters;
using Sonuts.Presentation.Services;

namespace Sonuts.Presentation;

public static class ConfigureServices
{
	public static IServiceCollection AddPresentationServices(this IServiceCollection services,
		IConfiguration configuration,
		IWebHostEnvironment environment)
	{ 
		if (environment.IsDevelopment())
			services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddSingleton<ICurrentUserService, CurrentUserService>();

		services.AddHttpContextAccessor();

		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>();

		services.AddControllersWithViews(options =>
			options.Filters.Add<ApiExceptionFilterAttribute>())
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
		services.AddFluentValidationClientsideAdapters();

		services.AddRazorPages();

		// Configure swagger API Docs
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = $"{environment.ApplicationName.Split('.').FirstOrDefault()}"
			});
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Example: 'Bearer {JWT-token}'",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
						Scheme = SecuritySchemeType.OAuth2.ToString(),
						Name = "Bearer",
						In = ParameterLocation.Header
					},
					Array.Empty<string>()
				}
			});
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
			options.CustomOperationIds(api => $"{api.ActionDescriptor.RouteValues["action"]}");
			options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Example = new OpenApiString(DateOnly.FromDateTime(DateTime.Now).ToLongDateString()) });
			options.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Example = new OpenApiString(TimeOnly.FromDateTime(DateTime.Now).ToLongTimeString()) });
			options.SchemaFilter<CustomSchemaFilter>();
		});

		// Configure JWT authentication
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = configuration["Authentication:Issuer"],
					ValidAudience = configuration["Authentication:Audience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecurityKey"]!))
				};
			});

		services.AddAuthorization(options =>
			{
				options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});

		return services;
	}

	public static WebApplication InitialiseAndSeedDatabase(this WebApplication app)
	{
		var task = Task.Run(async () =>
		{
			using var scope = app.Services.CreateScope();
			var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
			await initialiser.InitialiseAsync();
			await initialiser.SeedAsync();
		});
		task.Wait();
		return app;
	}
}
