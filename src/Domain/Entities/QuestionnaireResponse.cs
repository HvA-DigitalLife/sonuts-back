namespace Sonuts.Domain.Entities;

public class QuestionnaireResponse : BaseEntity
{
	public DateOnly CreatedAt { get; init; } = DateOnly.FromDateTime(DateTime.Now);
	public required Questionnaire Questionnaire { get; set; }
	public required Participant Participant { get; set; }
	public ICollection<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();
}
