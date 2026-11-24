using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Shared.DTO.Auth.Queries.HasUsernameAlreadyTaken
{
	public record HasUsernameAlreadyTakenRequest : IRequest<HasUsernameAlreadyTakenResponse>
	{
		public string Username { get; set; } = string.Empty;
	}
}
