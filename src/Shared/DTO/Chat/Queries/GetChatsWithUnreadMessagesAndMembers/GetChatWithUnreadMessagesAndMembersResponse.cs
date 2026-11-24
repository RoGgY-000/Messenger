using Shared.DTO.Chat.Queries.GetUnreadMessages;

namespace Shared.DTO.Chat.Queries.GetChatsWithUnreadMessagesAndMembers
{
	public sealed record GetChatWithUnreadMessagesAndMembersResponse
	{
		public Guid ChatId { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public bool IsGroup { get; set; }
		public int MemberCount { get; set; }
		public string Name { get; set; } = string.Empty;
		public Guid[] Members { get; set; } = [];
		public GetUnreadMessageResponse[] UnreadMessages { get; set; } = [];
	}
}