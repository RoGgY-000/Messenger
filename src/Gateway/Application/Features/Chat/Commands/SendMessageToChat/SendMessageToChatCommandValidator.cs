using FluentValidation;
using Messenger.Shared.Protos.Auth;
using Messenger.Shared.Protos.Chat;
using Shared.DTO.Chat.Commands.SendMessage;

namespace GatewayService.Application.Features.Chat.Commands.SendMessage
{
	public sealed class SendMessageToChatCommandValidator : AbstractValidator<SendMessageToChatCommand>
	{
		private readonly AuthServiceGrpc.AuthServiceGrpcClient _authGrpcClient;
		private readonly ChatServiceGrpc.ChatServiceGrpcClient _chatGrpcClient;
		public SendMessageToChatCommandValidator
			(AuthServiceGrpc.AuthServiceGrpcClient authGrpcClient,
			ChatServiceGrpc.ChatServiceGrpcClient chatGrpcClient)
		{
			_authGrpcClient = authGrpcClient;
			_chatGrpcClient = chatGrpcClient;

			RuleFor(c => c.ChatId)
				.NotEmpty()
				.WithMessage("Incorrect chat Id")
				.MustAsync(SendSearchChatByIdRequest)
				.WithMessage("Chat with this Id does not exist");

			RuleFor(c => c.SenderId)
				.NotEmpty()
				.WithMessage("Incorrect user Id")
				.MustAsync(SendSearchUserByIdRequest)
				.WithMessage("User with this Id does not exist");

			RuleFor(c => c.Text)
				.NotEmpty()
				.WithMessage("Message text can`t be empty")
				.MaximumLength(4096)
				.WithMessage("The message can be up to 4,096 characters long.");
		}

		private async Task<bool> SendSearchChatByIdRequest (Guid chatId, CancellationToken cancellationToken)
		{
			SearchChatByIdGrpcRequest request = new()
			{
				ChatId = chatId.ToString()
			};
			SearchChatByIdGrpcResponse response = await _chatGrpcClient.SearchChatByIdAsync(request, cancellationToken: cancellationToken);
			return response.IsExists;
		}

		private async Task<bool> SendSearchUserByIdRequest (Guid userId, CancellationToken cancellationToken)
		{
			SearchUserByIdGrpcRequest request = new()
			{
				UserId = userId.ToString()
			};
			SearchUserByIdGrpcResponse response = await _authGrpcClient.SearchUserByIdAsync(request, cancellationToken: cancellationToken);
			return response.IsExists;
		}
	}
}
