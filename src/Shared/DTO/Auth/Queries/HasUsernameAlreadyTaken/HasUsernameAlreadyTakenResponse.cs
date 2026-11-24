using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTO.Auth.Queries.HasUsernameAlreadyTaken
{
	public record HasUsernameAlreadyTakenResponse
	{
		public string Username { get; set; } = string.Empty;
		public bool HasTaken { get; set; } = false;
	}
}
