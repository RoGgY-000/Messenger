using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Shared.DTO.Auth.Queries.HasPhoneNumberAlreadyTaken
{
	public record HasPhoneNumberAlreadyTakenRequest : IRequest<HasPhoneNumberAlreadyTakenResponse>
	{
		public string PhoneNumber { get; set; } = string.Empty;
	}
}
