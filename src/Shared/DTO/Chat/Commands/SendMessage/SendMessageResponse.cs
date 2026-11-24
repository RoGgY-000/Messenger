namespace Shared.DTO.Chat.Commands.SendMessage
{
	public sealed record SendMessageResponse
	{
		public Guid ChatId { get; set; }
		public Guid ClientMessageId { get; set; }
		public Guid MessageId { get; set; }
		public bool IsSuccess { get; set; }
	}
}
