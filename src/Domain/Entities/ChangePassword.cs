using System;
namespace Sonuts.Domain.Entities;

public class ChangePassword : BaseEntity
{
	public required string Email { get; set; }
	public required string OldPassword { get; set; }
	public required string NewPassword { get; set; }
}

