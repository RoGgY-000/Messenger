using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTO.Auth.Queries.HasEmailAlreadyTaken
{
	public record HasEmailAlreadyTakenResponse
	{
		public string Email { get; set; } = string.Empty;
		public bool HasTaken { get; set; } = false;
	}
}
