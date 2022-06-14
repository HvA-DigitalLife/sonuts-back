namespace Sonuts.Domain.Entities;

public class Activity : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string? Video { get; set; }
	public Image Image { get; set; } = default!;
	public Theme Theme { get; set; } = default!;
	public QuestionDependency? QuestionDependency { get; set; }
}
