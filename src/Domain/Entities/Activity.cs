namespace Sonuts.Domain.Entities;

public class Activity : BaseEntity
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
	public Image Image { get; set; } = default!;
	public Theme Theme { get; set; } = default!;
	public List<Video> Video { get; set; } = new();
}
