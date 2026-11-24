using ChatService.Domain;
using ChatService.Infrastructure;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Chat.Commands.SendMessage;
using Shared.DTO.Chat.Events.MessageSent;

namespace ChatService.Application.Features.Commands.SendMessageToChat
{
	internal sealed class SendMessageToChatCommandHandler
		(ChatDbContext context,
		IPublishEndpoint publishEndpoint)
		: IRequestHandler<SendMessageToChatCommand, SendMessageResponse>
	{
		public async Task<SendMessageResponse> Handle (SendMessageToChatCommand request, CancellationToken cancellationToken)
		{
			SendMessageResponse response = new()
			{
				ClientMessageId = request.ClientMessageId,
				IsSuccess = false
			};

			if ( await context.ChatMembers.AnyAsync(c => c.ChatId == request.ChatId && c.UserId == request.SenderId, cancellationToken) )
			{
				Message message = new()
				{
					Id = Guid.CreateVersion7(),
					ChatId = request.ChatId,
					IsRead = false,
					SenderId = request.SenderId,
					SentAt = DateTime.UtcNow,
					Text = request.Text
				};
				context.Messages.Add(message);
				await context.SaveChangesAsync(cancellationToken);
				MessageSentEvent e = message.Adapt<MessageSentEvent>();
				await publishEndpoint.Publish(e,cancellationToken);
				response.MessageId = message.Id;
				response.IsSuccess = true;
				return response;
			}
			return response;
		}
	}
}
