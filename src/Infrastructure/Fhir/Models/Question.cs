namespace Sonuts.Infrastructure.Fhir.Models;

public class Question
{
	public string? Id { get; set; }

	public string? Text { get; set; }
        
	public string? Type { get; set; }

	public string? OpenLabel { get; set; }
	public List<QuestionAnswerOption> AnswerOptions { get; set; } = new List<QuestionAnswerOption>();
}
