using MediatR;

namespace Shared.DTO.Auth.Queries.VerifyPassword
{
	public record VerifyPasswordRequest : IRequest<VerifyPasswordResponse>
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
