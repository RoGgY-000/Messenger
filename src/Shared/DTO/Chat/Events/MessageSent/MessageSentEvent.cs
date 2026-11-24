namespace Shared.DTO.Chat.Events.MessageSent
{
	public sealed class MessageSentEvent
	{
		public Guid Id { get; set; }
		public Guid ChatId { get; set; }
		public bool IsRead { get; set; }
		public Guid SenderId { get; set; }
		public DateTime SentAt { get; set; }
		public string Text { get; set; } = string.Empty;
	}
}
