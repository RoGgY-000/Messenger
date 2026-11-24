using System.Linq;
using Mapster;
using Messenger.Shared.Protos.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;
using Shared.DTO.Chat.Queries.GetUnreadMessages;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.SignalR;

namespace ConnectionService.Hubs
{
	[Authorize]
	internal sealed class ChatHub
		(AuthServiceGrpc.AuthServiceGrpcClient authGrpcClient,
		ChatServiceGrpc.ChatServiceGrpcClient chatGrpcClient)
		: Hub
	{
		public override async Task OnConnectedAsync ()
		{
			await base.OnConnectedAsync();
			Console.WriteLine("connected client");
			if ( Context.User != null && Context.User.TryParseUserId(out Guid userId) )
			{
				GetChatWithLastMessageIdGrpcResponse[] chatsWithLastMessage = await GetUserChatsWithLastMessage(userId);
				await Clients.Caller.SendAsync(
					"ReceiveChatsWithLastMessage",
					chatsWithLastMessage);
				Console.WriteLine(chatsWithLastMessage.Length);

				foreach ( GetChatWithLastMessageIdGrpcResponse chat in chatsWithLastMessage )
				{
					await Groups.AddToGroupAsync(Context.ConnectionId, chat.ChatId.ToString());
				}
			}
		}

		public override async Task OnDisconnectedAsync (Exception? exception) => await base.OnDisconnectedAsync(exception);
		
		public async Task GetUnreadMessagesAndMembersForChats (Guid[] chats)
		{
			if (Context.User != null && Context.User.TryParseUserId(out Guid userId) && chats.Length > 0)
			{
				Console.WriteLine(chats.Length);
				ChatWithUnreadMessagesAndMembersGrpcResponse[] grpcResponse = await GetChatsWithUnreadMessagesAndMembers(userId, chats);
				List<Guid> usersIds = [];
				foreach ( ChatWithUnreadMessagesAndMembersGrpcResponse chat in grpcResponse )
				{
					usersIds.AddRange(chat.MembersIds.Select(id => Guid.Parse(id)));
					Console.WriteLine(chat.MemberCount);
					Console.WriteLine(chat.MembersIds.Count);
				}
				UserGrpcResponse[] users = await GetUsersById(usersIds.ToArray());
				Console.WriteLine(users.Length);
				await SendUnreadMessagesAndMembersForChats(grpcResponse, users);
			}
		}

		private async Task SendUnreadMessagesAndMembersForChats (ChatWithUnreadMessagesAndMembersGrpcResponse[] chats, UserGrpcResponse[] members)
		{
			Console.WriteLine(chats.Length);
			foreach ( ChatWithUnreadMessagesAndMembersGrpcResponse chat in chats )
			{
				Console.WriteLine(chat.MemberCount);
				Console.WriteLine(chat.MembersIds.Count);
			}
			Console.WriteLine(members.Length);

			ChatWithUnreadMessagesAndMembers[] result = chats.Select( chat =>
			new ChatWithUnreadMessagesAndMembers(
					chatId: Guid.Parse(chat.ChatId),
					isGroup: chat.IsGroup,
					imageUrl: chat.ImageUrl,
					memberCount: chat.MemberCount,
					name: chat.Name,
					members: chat.MembersIds.Select(id =>
					{
						UserGrpcResponse dto = members.Single(u => u.Id == id);
						return new User(Guid.Parse(id), dto.ImageUrl, dto.Name, dto.Username);
					}).ToArray(),
					unreadMessages: chat.UnreadMessages.Select(m => new UnreadMessage(Guid.Parse(m.ChatId), Guid.Parse(m.Id), Guid.Parse(m.SenderId), m.SentAt, m.Text)).ToArray()
				)
			).ToArray();

			Console.WriteLine(result.Length);

			await Clients.Caller.SendAsync(
				"ReceiveUnreadMessagesAndMembersForChats",
				result);
		}

		private async Task<ChatWithUnreadMessagesAndMembersGrpcResponse[]> GetChatsWithUnreadMessagesAndMembers (Guid userId, Guid[] chats)
		{
			GetChatsWithUnreadMessagesAndMembersGrpcRequest grpcRequest = new()
			{
				UserId = userId.ToString()
			};
			grpcRequest.Chats.AddRange(chats.Select(c => c.ToString()));
			GetChatsWithUnreadMessagesAndMembersGrpcResponse grpcResponse = await chatGrpcClient.GetChatsWithUnreadMessagesAndMembersAsync(grpcRequest);
			return grpcResponse.Chats.ToArray();
		}

		private async Task<UserGrpcResponse[]> GetUsersById (Guid[] usersIds)
		{
			GetUsersByIdGrpcRequest grpcRequest = new();
			grpcRequest.Users.AddRange(usersIds.Select(c => c.ToString()));
			GetUsersByIdGrpcResponse grpcResponse = await authGrpcClient.GetUsersByIdAsync(grpcRequest);
			return grpcResponse.Users.ToArray();
		}

		private async Task<GetChatWithLastMessageIdGrpcResponse[]> GetUserChatsWithLastMessage (Guid userId)
		{
			GetChatsWithLastMessageIdGrpcRequest grpcRequest = new()
			{
				UserId = userId.ToString()
			};
			GetChatsWithLastMessageIdGrpcResponse grpcResponse = await chatGrpcClient.GetUserChatsWithLastMessageIdAsync(grpcRequest);
			GetChatWithLastMessageIdGrpcResponse[] chats = grpcResponse.ChatsWithLastMessage.ToArray();
			return chats;
		}
	}
}
