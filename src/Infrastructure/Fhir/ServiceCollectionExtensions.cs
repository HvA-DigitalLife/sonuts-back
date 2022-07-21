using Microsoft.Extensions.DependencyInjection;

namespace Sonuts.Infrastructure.Fhir;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddFhir(this IServiceCollection services, FhirOptions options)
	{
		services.AddSingleton(options);

		return services;
	}

	public static IServiceCollection AddFhir(this IServiceCollection services, Action<FhirOptions> optionsAction)
	{
		FhirOptions options = new();
		optionsAction.Invoke(options);
		return services.AddFhir(options);
	}
}
