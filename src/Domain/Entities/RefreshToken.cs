namespace Sonuts.Domain.Entities;

public class RefreshToken
{
	public required string Token { get; set; }
	public DateTime IssuedAt { get; set; }
	public required User User { get; set; }
	public required Client Client { get; set; } 
}
