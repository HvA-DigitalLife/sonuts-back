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

if (app.Configuration.GetValue<bool>("Swagger"))
{
	app.UseSwagger();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/Health");
app.MapControllers();
app.MapRazorPages();

app.Run();

// Make the implicit Program class public so test projects can access it
namespace Sonuts.Presentation
{
	public partial class Program { }
}
