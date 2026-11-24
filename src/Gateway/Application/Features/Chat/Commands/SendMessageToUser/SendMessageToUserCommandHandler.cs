using Mapster;
using MediatR;
using Messenger.Shared.Protos.Chat;
using Shared.DTO.Chat.Commands.SendMessage;

namespace GatewayService.Application.Features.Chat.Commands.SendMessageToUser
{
	public sealed class SendMessageToUserCommandHandler
		(ChatServiceGrpc.ChatServiceGrpcClient grpcClient)
		: IRequestHandler<SendMessageToUserCommand, SendMessageResponse>
	{
		public async Task<SendMessageResponse> Handle (SendMessageToUserCommand request, CancellationToken cancellationToken)
		{
			SendMessageToUserGrpcRequest grpcRequest = request.Adapt<SendMessageToUserGrpcRequest>();
			SendMessageGrpcResponse grpcResponse = await grpcClient.SendMessageToUserAsync(grpcRequest, cancellationToken: cancellationToken);
			SendMessageResponse response = grpcResponse.Adapt<SendMessageResponse>();
			response.ChatId = Guid.Parse(grpcResponse.ChatId);
			return response;
		}
	}
}
