namespace Sonuts.Domain.Entities;

public class Client : BaseEntity
{
	public string Secret { get; set; } = default!;
	public string Name { get; set; } = default!;
}
