using Microsoft.AspNetCore.Identity;

namespace Sonuts.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
	public bool IsDeleted { get; set; } = false;
}
