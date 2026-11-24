using System.Collections.ObjectModel;

namespace Shared.DTO.Chat.Queries.GetChatsIdsByUserId
{
	public sealed record GetUserChatWithLastMessageIdResponse
	{
		public Guid ChatId { get; set; }
		public Guid LastMessageId { get; set; }
	}
}