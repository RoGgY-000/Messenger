using ConnectionService.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.DTO.Chat.Events.MessageSent;

namespace ConnectionService.Consumers
{
	internal class MessageSentConsumer (IHubContext<ChatHub> chatHubContext) : IConsumer<MessageSentEvent>
	{
		private readonly IHubContext<ChatHub> _chatHubContext = chatHubContext;

		public async Task Consume (ConsumeContext<MessageSentEvent> context)
		{
			MessageSentEvent ev = context.Message;
			var dto = new {
				chatId = ev.ChatId,
				id = ev.Id,
				senderId = ev.SenderId,
				sentAt = ev.SentAt.ToString("o"),
				text = ev.Text
			};
			await _chatHubContext.Clients.Group(ev.ChatId.ToString()).SendAsync(
				"ReceiveMessage",
				dto);
		}
	}
}
