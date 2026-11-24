using MediatR;

namespace Shared.DTO.Auth.Commands.SignUp
{
	public record SignUpCommand : IRequest<AuthResponse>
	{
		public string Username { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
	}
}
