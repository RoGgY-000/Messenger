using MediatR;

namespace Shared.DTO.Chat.Queries.GetUnreadMessages
{
	public sealed record GetUnreadMessagesQuery : IRequest<GetUnreadMessageResponse[]>
	{
		public Guid UserId { get; set; }

		public GetUnreadMessagesQuery (Guid userId) => UserId = userId;
	}
}
