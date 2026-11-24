using FluentValidation;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Commands.SignIn;

namespace Gateway.Application.Features.Auth.Commands.SignIn
{
	public class SignInCommandValidator : AbstractValidator<SignInCommand>
	{
		private readonly AuthServiceGrpc.AuthServiceGrpcClient _client;
		public SignInCommandValidator (AuthServiceGrpc.AuthServiceGrpcClient client)
		{
			_client = client;

			RuleFor(c => c.Username)
				.NotEmpty()
				.WithMessage("Username cannot be empty")
				.MinimumLength(4)
				.WithMessage("Username must contain at least 4 characters")
				.MaximumLength(256)
				.WithMessage("Username must contain up to 256 characters")
				.MustAsync(SendUsernameTakenRequest)
				.WithMessage("User with this username does not exists");

			RuleFor(c => c.Password)
				.NotEmpty()
				.WithMessage("Password cannot be empty")
				.MinimumLength(8)
				.WithMessage("Password must contain at least 8 characters")
				.MaximumLength(256)
				.WithMessage("Password must contain up to 256 characters");
		}

		private async Task<bool> SendUsernameTakenRequest (string username, CancellationToken cancellationToken)
		{
			HasUsernameAlreadyTakenGrpcRequest hasUsernameAlreadyTakenRequest = new()
			{
				Username = username
			};
			HasUsernameAlreadyTakenGrpcResponse response = await _client.HasUsernameAlreadyTakenAsync(hasUsernameAlreadyTakenRequest, cancellationToken: cancellationToken);
			return response.HasTaken;
		}
	}
}
