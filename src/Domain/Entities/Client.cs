namespace Sonuts.Domain.Entities;

public class Client : BaseEntity
{
	public required string Secret { get; set; }
	public required string Name { get; set; }
}
