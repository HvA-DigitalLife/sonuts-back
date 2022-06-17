namespace Sonuts.Domain.Entities;

public class Content : BaseEntity
{
	public ContentType Type { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Subtitle { get; set; } = default!;
	public string Description { get; set; } = default!;
}
