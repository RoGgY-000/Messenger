using FluentValidation;
using Mapster;
using MediatR;
using Messenger.Shared.Protos.Chat;
using Shared.DTO.Chat.Commands.SendMessage;

namespace GatewayService.Application.Features.Chat.Commands.SendMessage
{
	public class SendMessageToChatCommandHandler
		(ChatServiceGrpc.ChatServiceGrpcClient grpcClient)
		: IRequestHandler<SendMessageToChatCommand, SendMessageResponse>
	{
		public async Task<SendMessageResponse> Handle (SendMessageToChatCommand request, CancellationToken cancellationToken)
		{
			SendMessageToChatGrpcRequest grpcRequest = request.Adapt<SendMessageToChatGrpcRequest>();
			SendMessageGrpcResponse grpcResponse = await grpcClient.SendMessageToChatAsync(grpcRequest);
			SendMessageResponse response = grpcResponse.Adapt<SendMessageResponse>();
			return response;
		}
	}
}
