namespace Sonuts.Domain.Entities;

public class QuestionnaireResponse : BaseEntity
{
	public DateTime CreatedAt { get; init; } = DateTime.Now;
	public Questionnaire Questionnaire { get; set; } = default!;
	public Participant Participant { get; set; } = default!;
	public ICollection<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();
}
