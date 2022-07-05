namespace Sonuts.Infrastructure.Fhir.Models;

public class Guideline
{

	public string? Id { get; set; }

	public string? DomainId { get; set; }
  
	public string? Title { get; set; }


	public string? Text { get; set; }

	public List<Goal> Goals { get; set; } = new List<Goal>();

}
