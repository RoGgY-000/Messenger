using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Shared.DTO.Chat.Queries.GetChatsWithUnreadMessagesAndMembers
{
	public sealed record GetChatsWithUnreadMessagesAndMembersQuery : IRequest<GetChatWithUnreadMessagesAndMembersResponse[]>
	{
		public Guid UserId { get; set; }
		public Guid[] Chats { get; set; } = [];
	}
}
