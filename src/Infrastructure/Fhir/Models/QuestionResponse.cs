namespace Sonuts.Infrastructure.Fhir.Models;

public class QuestionResponse
{        

	public string? QuestionId { get; set; }

	public string? Response { get; set; }

	public List<string> ChoiceOptionIds { get; set; } = new List<string>();

}
