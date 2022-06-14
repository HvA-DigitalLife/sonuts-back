using Sonuts.Application;
using Sonuts.Infrastructure;
using Sonuts.Infrastructure.Persistence;
using Sonuts.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddPresentationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

	// Initialise and seed database
	using var scope = app.Services.CreateScope();
	var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
	await initialiser.InitialiseAsync();
	await initialiser.SeedAsync();
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();

// Make the implicit Program class public so test projects can access it
namespace Sonuts.Presentation
{
	public partial class Program { }
}
