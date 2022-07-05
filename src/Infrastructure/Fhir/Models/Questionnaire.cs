namespace Sonuts.Infrastructure.Fhir.Models.disabled;

public class Questionnaire
{
	public string? Id { get; set; }

	public string? Title  { get; set; }

	public string? Description  { get; set; }

	public List<Question> Questions { get; set; } = new List<Question>(); 

}
