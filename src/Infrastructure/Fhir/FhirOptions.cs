using Sonuts.Application.Common.Interfaces.Fhir;

namespace Sonuts.Infrastructure.Fhir;

public record FhirOptions : IFhirOptions
{
	public bool Read { get; set; }
	public bool Write { get; set; }
}
