namespace Sonuts.Domain.Entities;

public class Questionnaire : BaseEntity
{
	public string Title { get; set; } = default!;

	public string? Description { get; set; }

	public List<Question> Questions { get; set; } = new List<Question>();
}
