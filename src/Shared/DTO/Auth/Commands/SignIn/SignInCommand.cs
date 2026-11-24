using MediatR;

namespace Shared.DTO.Auth.Commands.SignIn
{
	public record SignInCommand : IRequest<AuthResponse>
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
