using ChatService.Domain;
using ChatService.Infrastructure;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Chat.Commands.SendMessage;
using Shared.DTO.Chat.Events.MessageSent;

namespace ChatService.Application.Features.Commands.SendMessageToUser
{
	internal sealed class SendMessageToUserCommandHandler
		(ChatDbContext context,
		IPublishEndpoint publishEndpoint)
		: IRequestHandler<SendMessageToUserCommand, SendMessageResponse>
	{
		public async Task<SendMessageResponse> Handle (SendMessageToUserCommand request, CancellationToken cancellationToken)
		{
			Chat? chat = await context.Chats.SingleOrDefaultAsync(
				c => context.ChatMembers.Any(cm => cm.UserId == request.SenderId && cm.ChatId == c.Id)
				&& context.ChatMembers.Any(cm => cm.UserId == request.RecipientId && cm.ChatId == c.Id)
				&& c.IsGroup == false, cancellationToken);
			if ( chat == null )
			{
				chat = new()
				{
					Id = Guid.CreateVersion7(),
					IsGroup = false,
					Name = string.Empty
				};
				context.Chats.Add(chat);

				ChatMember senderMember = new()
				{
					ChatId = chat.Id,
					Id = Guid.CreateVersion7(),
					JoinedAt = DateTime.UtcNow,
					UserId = request.SenderId
				};
				ChatMember recipientMember = new()
				{
					ChatId = chat.Id,
					Id = Guid.CreateVersion7(),
					JoinedAt = DateTime.UtcNow,
					UserId = request.RecipientId
				};
				context.ChatMembers.AddRange(senderMember,  recipientMember);
			}

			Message message = new()
			{
				ChatId = chat.Id,
				Id = Guid.CreateVersion7(),
				IsRead = false,
				SenderId = request.SenderId,
				SentAt = DateTime.UtcNow,
				Text = request.Text
			};
			context.Messages.Add(message);

			await context.SaveChangesAsync(cancellationToken);

			MessageSentEvent e = message.Adapt<MessageSentEvent>();
			await publishEndpoint.Publish(e, cancellationToken);

			SendMessageResponse response = new()
			{
				ChatId = chat.Id,
				ClientMessageId = request.ClientMessageId,
				IsSuccess = true,
				MessageId = message.Id
			};
			return response;
		}
	}
}
