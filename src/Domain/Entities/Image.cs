namespace Sonuts.Domain.Entities;

public class Image : BaseEntity
{
	public required string Extension { get; set; }
	public string? Name { get; set; }
}
