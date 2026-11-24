using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTO.Auth.Queries.HasPhoneNumberAlreadyTaken
{
	public record HasPhoneNumberAlreadyTakenResponse
	{
		public string PhoneNumber { get; set; } = string.Empty;
		public bool HasTaken { get; set; } = false;
	}
}
