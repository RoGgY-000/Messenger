using ChatService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Shared.DTO.Chat.Queries.GetChatsWithUnreadMessagesAndMembers;
using Mapster;
using Shared.DTO.Chat.Queries.GetUnreadMessages;

namespace ChatService.Application.Features.Queries.GetChatsWithUnreadMessagesAndMembers
{
	internal sealed class GetChatsWithUnreadMessagesAndMembersQueryHandler
		(ChatDbContext context)
		: IRequestHandler<GetChatsWithUnreadMessagesAndMembersQuery, GetChatWithUnreadMessagesAndMembersResponse[]>
	{
		public async Task<GetChatWithUnreadMessagesAndMembersResponse[]> Handle (GetChatsWithUnreadMessagesAndMembersQuery request, CancellationToken cancellationToken)
		{
			return await context.Chats
				.Where(c =>
				context.ChatMembers.Any(cm => cm.ChatId == c.Id && cm.UserId == request.UserId)
				&& context.Messages.Any(m => m.ChatId == c.Id && !m.IsRead && m.SenderId != request.UserId))
				.Select(c => new GetChatWithUnreadMessagesAndMembersResponse()
				{
					ChatId = c.Id,
					ImageUrl = string.Empty,
					IsGroup = false,
					MemberCount = context.ChatMembers.Where(cm => cm.ChatId == c.Id).Count(),
					Name = c.Name,
					Members = context.ChatMembers.Where(cm => cm.ChatId == c.Id).Select(cm => cm.UserId).ToArray(),
					UnreadMessages = context.Messages
					.Where(m => m.ChatId == c.Id && !m.IsRead && m.SenderId != request.UserId)
					.Select( m => new GetUnreadMessageResponse
					{
						ChatId = m.ChatId,
						Id = m.Id,
						SenderId = m.SenderId,
						SentAt = m.SentAt,
						Text = m.Text
					})
					.ToArray()
				})
				.ToArrayAsync(cancellationToken);

		}
	}
}
