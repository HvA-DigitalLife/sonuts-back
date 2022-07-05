namespace Sonuts.Infrastructure.Fhir.Models;

public class Participant
{
	public string? Id { get; set; }
	public string? Pseudonym { get; set; }
	public string? EmailAddress { get; set; }
	public string? Password { get; set; }
	public byte Age { get; set; }
	public string? Gender { get; set; }
	public ushort Weight { get; set; }
	public ushort Height { get; set; }
	public string? MaritalStatus { get; set; }
	public bool IsActive { get; set; }
	public string? FhirId  { get; set; }
	public string? KeyCloakId  { get; set; }
}
