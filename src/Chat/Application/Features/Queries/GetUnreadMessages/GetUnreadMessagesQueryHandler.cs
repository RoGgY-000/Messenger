using ChatService.Infrastructure;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Chat.Queries.GetUnreadMessages;

namespace ChatService.Application.Features.Queries.GetUnreadMessages
{
	internal class GetUnreadMessagesQueryHandler
		(ChatDbContext context)
		: IRequestHandler<GetUnreadMessagesQuery, GetUnreadMessageResponse[]>
	{
		public async Task<GetUnreadMessageResponse[]> Handle (GetUnreadMessagesQuery request, CancellationToken cancellationToken)
		{
			GetUnreadMessageResponse[] result = 
				await context.Messages
				.AsNoTracking()
				.Where(m =>
				m.IsRead == false
				&& context.ChatMembers.Any(cm => cm.UserId == request.UserId && cm.ChatId == m.ChatId))
				.OrderBy(msg => msg.SentAt)
				.Take(100)
				.ProjectToType<GetUnreadMessageResponse>()
				.ToArrayAsync(cancellationToken);
			return result;
		}
	}
}