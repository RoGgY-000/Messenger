namespace ChatService.Domain
{
	public sealed class ChatMember
	{
		public Guid Id {  get; set; }
		public Guid ChatId { get; set; }
		public Guid UserId { get; set; }
		public DateTime JoinedAt { get; set; }
	}
}
