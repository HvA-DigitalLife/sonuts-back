using System;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos
{
	public class ChangePasswordDto : Common.Mappings.IMapFrom<ChangePassword>
	{
		public required string Email { get; set; }
		public required string OldPassword { get; set; }
		public required string NewPassword { get; set; }
	}
}

