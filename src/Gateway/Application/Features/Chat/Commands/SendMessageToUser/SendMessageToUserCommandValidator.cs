using FluentValidation;
using Messenger.Shared.Protos.Auth;
using Messenger.Shared.Protos.Chat;
using Shared.DTO.Chat.Commands.SendMessage;

namespace GatewayService.Application.Features.Chat.Commands.SendMessageToUser
{
	public class SendMessageToUserCommandValidator
		: AbstractValidator<SendMessageToUserCommand>
	{
		private readonly AuthServiceGrpc.AuthServiceGrpcClient _client;
		public SendMessageToUserCommandValidator (AuthServiceGrpc.AuthServiceGrpcClient grpcClient)
		{
			_client = grpcClient;

			RuleFor(c => c.ClientMessageId)
				.NotEmpty()
				.WithMessage("Incorrent client message id");

			RuleFor(c => c.RecipientId)
				.NotEmpty()
				.WithMessage("Incorrent recipient id")
				.MustAsync(SendSearchUserByIdRequest)
				.WithMessage("Recipient with this id does not exist");

			RuleFor(c => c.SenderId)
				.NotEmpty()
				.WithMessage("Incorrent sender id")
				.MustAsync(SendSearchUserByIdRequest)
				.WithMessage("Sender with this id does not exist");

			RuleFor(c => c.Text)
				.NotEmpty()
				.WithMessage("Message text cannot be empty")
				.MaximumLength(4096)
				.WithMessage("The message can be up to 4,096 characters long.");
		}

		private async Task<bool> SendSearchUserByIdRequest (Guid userId, CancellationToken cancellationToken)
		{
			SearchUserByIdGrpcRequest request = new()
			{
				UserId = userId.ToString()
			};
			SearchUserByIdGrpcResponse response = await _client.SearchUserByIdAsync(request, cancellationToken: cancellationToken);
			return response.IsExists;
		}
	}
}
