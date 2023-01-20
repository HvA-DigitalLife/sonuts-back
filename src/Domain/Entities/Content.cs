namespace Sonuts.Domain.Entities;

public class Content : BaseEntity
{
	public required ContentType Type { get; set; }
	public required string Title { get; set; }
	public required string Subtitle { get; set; }
	public required string Description { get; set; }
}
