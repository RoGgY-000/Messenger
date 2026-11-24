using MediatR;

namespace Shared.DTO.Chat.Queries.SearchChatById
{
	public sealed record SearchChatByIdQuery : IRequest<SearchChatByIdResponse>
	{
		public Guid ChatId { get; set; }
	}
}
