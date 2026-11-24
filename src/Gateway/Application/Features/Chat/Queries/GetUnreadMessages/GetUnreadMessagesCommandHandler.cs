using Mapster;
using MediatR;
using Messenger.Shared.Protos.Chat;
using Shared.DTO.Chat.Queries.GetUnreadMessages;

namespace GatewayService.Application.Features.Chat.Query.GetUnreadMessages
{
	//public class GetUnreadMessagesCommandHandler
	//	(ChatServiceGrpc.ChatServiceGrpcClient grpcClient)
	//	: IRequestHandler<GetUnreadMessagesQuery, GetUnreadMessageResponse[]>
	//{
	//	public async Task<GetUnreadMessageResponse[]> Handle (GetUnreadMessagesQuery request, CancellationToken cancellationToken)
	//	{
	//		GetUnreadMessagesGrpcRequest grpcRequest = request.Adapt<GetUnreadMessagesGrpcRequest>();
	//		GetUnreadMessagesGrpcResponse grpcResponse = await grpcClient.GetUnreadMessagesAsync(grpcRequest, cancellationToken: cancellationToken);
	//		GetUnreadMessageResponse[] response = grpcResponse.Messages.Adapt<GetUnreadMessageResponse[]>();
	//		return response;
	//	}
	//}
}
