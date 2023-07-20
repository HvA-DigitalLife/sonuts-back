using Microsoft.Extensions.FileProviders;
using Sonuts.Application;
using Sonuts.Infrastructure;
using Sonuts.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.InitialiseAndSeedDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseHsts();
}

if (app.Configuration.GetValue<bool>("Swagger"))
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(app.Configuration["Files:ImagePath"]!),
	RequestPath = "/Images"
});

app.UseRouting();

app.UseHealthChecks("/Health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();

// Make the implicit Program class public so test projects can access it
namespace Sonuts.Presentation
{
	public partial class Program { }
}
