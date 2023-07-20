using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Sonuts.Application.Common.Documentation;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sonuts.Presentation.Common;

public class CustomSchemaFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		foreach (var propertyInfo in context.Type.GetProperties())
		{
			if (propertyInfo.CustomAttributes.FirstOrDefault(customAttribute => customAttribute.AttributeType.Name == nameof(ExampleAttribute)) is { } exampleAttribute)
				schema.Properties
					.First(property => property.Key.ToLower().Equals(propertyInfo.Name.ToLower()))
					.Value
					.Example = new OpenApiString(exampleAttribute.ConstructorArguments.First().Value?.ToString() ?? string.Empty);
		}
	}
}
