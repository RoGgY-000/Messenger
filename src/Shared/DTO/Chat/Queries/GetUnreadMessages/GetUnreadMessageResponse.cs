namespace Shared.DTO.Chat.Queries.GetUnreadMessages
{
	public sealed record GetUnreadMessageResponse
	{
		public Guid Id { get; set; }
		public Guid ChatId { get; set; }
		public Guid SenderId { get; set; }
		public DateTime SentAt { get; set; }
		public string Text { get; set; } = string.Empty;
	}
}
