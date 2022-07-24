namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IFhirOptions
{
    bool Read { get; set; }
	bool Write { get; set; }
}
