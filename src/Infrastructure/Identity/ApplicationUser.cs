using Microsoft.AspNetCore.Identity;

namespace Sonuts.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
	public DateTime CreatedAt { get; set; } = DateTime.Now;
	public bool IsDeleted { get; set; } = false;
}
