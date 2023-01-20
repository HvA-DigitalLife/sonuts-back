namespace Sonuts.Domain.Entities;

public class QuestionnaireResponse : BaseEntity
{
	public DateTime CreatedAt { get; init; } = DateTime.Now;
	public required Questionnaire Questionnaire { get; set; }
	public required Participant Participant { get; set; }
	public ICollection<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();
}
