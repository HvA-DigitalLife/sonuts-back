namespace Sonuts.Domain.Entities;

public class Image : BaseEntity
{
	public string Extension { get; set; } = default!;
	public string? Name { get; set; }
}
