using ChatService.Infrastructure;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Chat.Queries.GetChatsIdsByUserId;

namespace ChatService.Application.Features.Queries.GetChatsIdsByUserId
{
	internal sealed class GetUserChatsWithLastMessageIdQueryHandler
		(ChatDbContext context)
		: IRequestHandler<GetUserChatsWithLastMessageIdQuery, GetUserChatWithLastMessageIdResponse[]>
	{
		public async Task<GetUserChatWithLastMessageIdResponse[]> Handle (GetUserChatsWithLastMessageIdQuery request, CancellationToken cancellationToken)
		{
			return await context.Chats
				.Where(c =>
				context.ChatMembers.Any(cm => cm.ChatId == c.Id && cm.UserId == request.UserId)
				&& context.Messages.Any(m => m.ChatId == c.Id && !m.IsRead && m.SenderId != request.UserId))
				.Select(c => new GetUserChatWithLastMessageIdResponse() { ChatId = c.Id, LastMessageId = context.Messages.Where(m => m.ChatId == c.Id).OrderByDescending(m => m.Id).First().Id})
				.ToArrayAsync(cancellationToken);
		}
	}
}
