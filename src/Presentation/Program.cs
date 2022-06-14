using Sonuts.Application;
using Sonuts.Infrastructure;
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
    app.InitialiseAndSeedDatabase();
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

app.Run();

// Make the implicit Program class public so test projects can access it
namespace Sonuts.Presentation
{
	public partial class Program { }
}
