namespace Shared.DTO.Chat.Queries.SearchChatById
{
	public sealed record SearchChatByIdResponse
	{
		public Guid ChatId { get; set; }
		public bool IsExists { get; set; }
	}
}
