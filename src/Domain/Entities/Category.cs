namespace Sonuts.Domain.Entities;

public class Category : BaseEntity
{
	public bool IsActive { get; set; } = false;
	public required string Name { get; set; }
	public required string Color { get; set; }
	public required Questionnaire Questionnaire { get; set; }
	public ICollection<Theme> Themes { get; set; } = new List<Theme>();
}
