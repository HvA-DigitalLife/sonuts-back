namespace Sonuts.Domain.Entities;

public class Category : BaseEntity
{
	public bool IsActive { get; set; } = false;
	public string Name { get; set; } = default!;
	public string Color { get; set; } = default!;
	public Questionnaire Questionnaire { get; set; } = default!;
	public ICollection<Theme> Themes { get; set; } = new List<Theme>();
}
