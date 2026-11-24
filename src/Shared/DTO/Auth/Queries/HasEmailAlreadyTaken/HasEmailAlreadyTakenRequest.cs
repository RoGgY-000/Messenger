using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Shared.DTO.Auth.Queries.HasEmailAlreadyTaken
{
	public record HasEmailAlreadyTakenRequest : IRequest<HasEmailAlreadyTakenResponse>
	{
		public string Email { get; set; } = string.Empty;
	}
}
