namespace Sonuts.Infrastructure.Fhir;

public record FhirOptions
{
	public bool Read { get; set; }
	public bool Write { get; set; }
}
