using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Shared.DTO.Auth.Commands.Refresh
{
	public record RefreshCommand : IRequest<AuthResponse>
	{
		public Guid RefreshToken { get; set; }
		public Guid UserId { get; set; }
	}
}
