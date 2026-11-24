using FluentValidation;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Chat.Queries.GetUnreadMessages;

namespace GatewayService.Application.Features.Chat.Query.GetUnreadMessages
{
	public sealed class GetUnreadMessagesQueryValidator : AbstractValidator<GetUnreadMessagesQuery>
	{
		private readonly AuthServiceGrpc.AuthServiceGrpcClient _client;
		public GetUnreadMessagesQueryValidator
			(AuthServiceGrpc.AuthServiceGrpcClient grpcClient)
		{
			_client = grpcClient;

			RuleFor(c => c.UserId)
				.MustAsync(SendSearchUserByIdRequest)
				.WithMessage("User with this id does not exist");
		}

		private async Task<bool> SendSearchUserByIdRequest (Guid userId, CancellationToken cancellationToken)
		{
			SearchUserByIdGrpcRequest grpcRequest = new()
			{
				UserId = userId.ToString()
			};
			SearchUserByIdGrpcResponse grpcResponse = await _client.SearchUserByIdAsync(grpcRequest, cancellationToken: cancellationToken);
			return grpcResponse.IsExists;
		}
	}
}
