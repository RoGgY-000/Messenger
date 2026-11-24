using Grpc.Core;
using Mapster;
using MediatR;
using Messenger.Shared.Protos.Chat;
using Shared.DTO.Chat.Commands.SendMessage;
using Shared.DTO.Chat.Queries.GetChatsIdsByUserId;
using Shared.DTO.Chat.Queries.GetChatsWithUnreadMessagesAndMembers;
using Shared.DTO.Chat.Queries.GetUnreadMessages;
using Shared.DTO.Chat.Queries.SearchChatById;

namespace ChatService.Application.Services
{
	internal sealed class GrpcChatService
		(ISender mediatr)
		: ChatServiceGrpc.ChatServiceGrpcBase
	{
		public override async Task<GetChatsWithLastMessageIdGrpcResponse> GetUserChatsWithLastMessageId (GetChatsWithLastMessageIdGrpcRequest grpcRequest, ServerCallContext context)
		{
			GetUserChatsWithLastMessageIdQuery request = grpcRequest.Adapt<GetUserChatsWithLastMessageIdQuery>();
			GetUserChatWithLastMessageIdResponse[] response = await mediatr.Send(request, context.CancellationToken);
			GetChatsWithLastMessageIdGrpcResponse grpcResponse = new();
			grpcResponse.ChatsWithLastMessage.AddRange(response.Select(chat =>
			new GetChatWithLastMessageIdGrpcResponse()
			{
				ChatId = chat.ChatId.ToString(),
				LastMessageId = chat.LastMessageId.ToString()
			}));
			return grpcResponse;
		}

		public override async Task<GetChatsWithUnreadMessagesAndMembersGrpcResponse> GetChatsWithUnreadMessagesAndMembers (GetChatsWithUnreadMessagesAndMembersGrpcRequest grpcRequest, ServerCallContext context)
		{
			GetChatsWithUnreadMessagesAndMembersQuery request = new()
			{
				UserId = Guid.Parse(grpcRequest.UserId),
				Chats = grpcRequest.Chats.Select(id => Guid.Parse(id)).ToArray()
			};
			GetChatWithUnreadMessagesAndMembersResponse[] response = await mediatr.Send(request, context.CancellationToken);
			GetChatsWithUnreadMessagesAndMembersGrpcResponse grpcResponse = new();
			grpcResponse.Chats.AddRange(response.Select(chat =>
			{
				ChatWithUnreadMessagesAndMembersGrpcResponse grpcChat = new()
				{
					ChatId = chat.ChatId.ToString(),
					ImageUrl = chat.ImageUrl,
					IsGroup = chat.IsGroup,
					MemberCount = chat.MemberCount,
					Name = chat.Name
				};
				grpcChat.MembersIds.AddRange(chat.Members.Select(id => id.ToString()));
				grpcChat.UnreadMessages.AddRange(chat.UnreadMessages.Adapt<UnreadMessageGrpcResponse[]>());
				return grpcChat;
			}));
			return grpcResponse;
		}

		public override async Task<SearchChatByIdGrpcResponse> SearchChatById (SearchChatByIdGrpcRequest grpcRequest, ServerCallContext context)
		{
			SearchChatByIdQuery request = grpcRequest.Adapt<SearchChatByIdQuery>();
			SearchChatByIdResponse response = await mediatr.Send (request, context.CancellationToken);
			SearchChatByIdGrpcResponse grpcResponse = response.Adapt<SearchChatByIdGrpcResponse>();
			return grpcResponse;
		}

		public override async Task<SendMessageGrpcResponse> SendMessageToChat (SendMessageToChatGrpcRequest grpcRequest, ServerCallContext context)
		{
			SendMessageToChatCommand request = grpcRequest.Adapt<SendMessageToChatCommand>();
			SendMessageResponse response = await mediatr.Send (request, context.CancellationToken);
			SendMessageGrpcResponse grpcResponse = response.Adapt<SendMessageGrpcResponse>();
			return grpcResponse;
		}

		public override async Task<SendMessageGrpcResponse> SendMessageToUser (SendMessageToUserGrpcRequest grpcRequest, ServerCallContext context)
		{
			SendMessageToUserCommand request = grpcRequest.Adapt<SendMessageToUserCommand>();
			SendMessageResponse response = await mediatr.Send(request, context.CancellationToken);
			SendMessageGrpcResponse grpcResponse = response.Adapt<SendMessageGrpcResponse>();
			return grpcResponse;
		}
	}
}