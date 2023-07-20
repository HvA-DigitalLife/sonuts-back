namespace Sonuts.Domain.Entities;

public class QuestionResponse : BaseEntity
{
	public QuestionnaireResponse QuestionnaireResponse { get; set; } = default!;
	public Question Question { get; set; } = default!;
	public required string Answer { get; set; }
}
