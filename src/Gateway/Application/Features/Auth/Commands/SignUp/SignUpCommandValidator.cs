using FluentValidation;
using Shared.DTO.Auth.Commands.SignUp;
using Messenger.Shared.Protos.Auth;

namespace Gateway.Application.Features.Auth.Commands.SignUp
{
	public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
	{
		private readonly AuthServiceGrpc.AuthServiceGrpcClient _client;
		public SignUpCommandValidator(AuthServiceGrpc.AuthServiceGrpcClient client)
		{
			_client = client;

			RuleFor(c => c.Username)
				.NotEmpty()
				.WithMessage("Username cannot be empty")
				.MinimumLength(4)
				.WithMessage("Username must contain at least 4 characters")
				.MaximumLength(256)
				.WithMessage("Username must contain up to 256 characters")
				.MustAsync(SendUsernameNotTakenRequest)
				.WithMessage("This username has already taken");

			RuleFor(c => c.Name)
				.NotEmpty()
				.WithMessage("Name cannot be empty")
				.MinimumLength(4)
				.WithMessage("Name must contain at least 4 characters")
				.MaximumLength(256)
				.WithMessage("Name must contain up to 256 characters");

			RuleFor(c => c.Password)
				.NotEmpty()
				.WithMessage("Password cannot be empty")
				.MinimumLength(8)
				.WithMessage("Password must contain at least 8 characters")
				.MaximumLength(256)
				.WithMessage("Password must contain up to 256 characters");

			RuleFor(c => c.Email)
				.EmailAddress()
				.WithMessage("Incorrect email format")
				.MustAsync(SendEmailNotTakenRequest)
				.WithMessage("This email has already taken");

			When(c => !string.IsNullOrWhiteSpace(c.PhoneNumber),
				() => RuleFor(c => c.PhoneNumber)
					.Matches(@"^\+?[1-9]\d{1,14}$")
					.WithMessage("Incorrect phone number format")
					.MustAsync(SendPhoneNumberNotTakenRequest)
					.WithMessage("Incorrect phone number format"));
		}

		private async Task<bool> SendUsernameNotTakenRequest (string username, CancellationToken cancellationToken)
		{
			HasUsernameAlreadyTakenGrpcRequest hasUsernameAlreadyTakenRequest = new()
			{
				Username = username
			};
			HasUsernameAlreadyTakenGrpcResponse response = await _client.HasUsernameAlreadyTakenAsync(hasUsernameAlreadyTakenRequest, cancellationToken: cancellationToken);
			return !response.HasTaken;
		}

		private async Task<bool> SendEmailNotTakenRequest (string email, CancellationToken cancellationToken)
		{
			HasEmailAlreadyTakenGrpcRequest hasEmailAlreadyTakenRequest = new()
			{
				Email = email
			};
			HasEmailAlreadyTakenGrpcResponse response = await _client.HasEmailAlreadyTakenAsync(hasEmailAlreadyTakenRequest, cancellationToken: cancellationToken);
			return !response.HasTaken;
		}

		private async Task<bool> SendPhoneNumberNotTakenRequest (string phoneNumber, CancellationToken cancellationToken)
		{
			HasPhoneNumberAlreadyTakenGrpcRequest hasPhoneNumberAlreadyTakenRequest = new()
			{
				PhoneNumber = phoneNumber
			};
			HasPhoneNumberAlreadyTakenGrpcResponse response = await _client.HasPhoneNumberAlreadyTakenAsync(hasPhoneNumberAlreadyTakenRequest, cancellationToken: cancellationToken);
			return !response.HasTaken;
		}
	}
}
