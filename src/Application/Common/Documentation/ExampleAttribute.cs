namespace Sonuts.Application.Common.Documentation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExampleAttribute : Attribute
{
	public ExampleAttribute(string value)
	{
		Value = value;
	}

	public string Value { get; init; }
}
