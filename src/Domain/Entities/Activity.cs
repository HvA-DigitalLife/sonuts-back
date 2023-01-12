namespace Sonuts.Domain.Entities;

public class Activity : BaseEntity
{
	public required string Name { get; set; }
	public string? Description { get; set; }
	public required Image Image { get; set; }
	public Theme Theme { get; set; } = default!;
	public List<Video> Videos { get; set; } = new();
}
