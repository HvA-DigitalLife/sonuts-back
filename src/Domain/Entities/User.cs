using Microsoft.AspNetCore.Identity;

namespace Sonuts.Domain.Entities;

public class User : IdentityUser
{
	public DateTime CreatedAt { get; set; } = DateTime.Now;
	public bool IsDeleted { get; set; } = false;
	public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
