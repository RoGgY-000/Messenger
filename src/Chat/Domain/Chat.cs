namespace ChatService.Domain
{
	public sealed class Chat
	{
		public Guid Id { get; set; }
		public bool IsGroup { get; set; } = false;
		public string Name { get; set; } = string.Empty;
	}
}
