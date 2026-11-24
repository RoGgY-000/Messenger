using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTO.SignalR
{
	public sealed record User (Guid id, string imageUrl, string name, string username);

	public sealed record UnreadMessage (Guid chatId, Guid id, Guid senderId, string sentAt, string text);

	public sealed record ChatWithUnreadMessagesAndMembers (Guid chatId, bool isGroup, string imageUrl, int memberCount, string name, User[] members, UnreadMessage[] unreadMessages);
}
