namespace Sonuts.Infrastructure.Fhir.Models;

public class Caregiver
{
	public string? Id { get; set; }
	public string? Name { get; set; }
	public bool IsComplete { get; set; }
	public string? Secret { get; set; }
}
