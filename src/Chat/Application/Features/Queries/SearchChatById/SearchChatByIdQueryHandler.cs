using ChatService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Chat.Queries.SearchChatById;

namespace ChatService.Application.Features.Queries.SearchChatById
{
	internal sealed class SearchChatByIdQueryHandler
		(ChatDbContext context)
		: IRequestHandler<SearchChatByIdQuery, SearchChatByIdResponse>
	{
		public async Task<SearchChatByIdResponse> Handle (SearchChatByIdQuery request, CancellationToken cancellationToken)
		{
			bool exists = await context.Chats.AnyAsync(c => c.Id == request.ChatId, cancellationToken);
			SearchChatByIdResponse response = new()
			{
				ChatId = request.ChatId,
				IsExists = exists
			};
			return response;
		}
	}
}