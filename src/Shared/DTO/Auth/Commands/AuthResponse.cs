using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTO.Auth.Commands
{
	public record AuthResponse
	{
		public Guid UserId { get; set; } = Guid.Empty;
		public string AccessToken { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = string.Empty;
		public DateTime ExpiresAt { get; set; }
	}
}
