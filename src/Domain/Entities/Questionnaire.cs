namespace Sonuts.Domain.Entities;

public class Questionnaire : BaseEntity
{
	public required string Title { get; set; }
	public string? Description { get; set; }
	public ICollection<Question> Questions { get; set; } = new List<Question>();
}
