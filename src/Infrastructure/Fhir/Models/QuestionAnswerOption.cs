

namespace Sonuts.Infrastructure.Fhir.Models;

public class QuestionAnswerOption
{
	public string? Id { get; set; }

	public string Text { get; set; } = default!;
        
	public bool Selected { get; set; }

   
}
