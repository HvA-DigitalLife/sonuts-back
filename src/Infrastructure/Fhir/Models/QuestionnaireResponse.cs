namespace Sonuts.Infrastructure.Fhir.Models;

public class QuestionnaireResponse
{

	public string? Id { get; set; }

	public string QuestionnaireId { get; set; } = default!;

	public string ParticipantId { get; set; } = default!;

	public List<QuestionResponse> QuestionResponses { get; set; } = new List<QuestionResponse>();
}
