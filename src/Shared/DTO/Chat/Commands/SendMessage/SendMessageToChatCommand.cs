using MediatR;
using Messenger.Shared.Protos.Chat;

namespace Shared.DTO.Chat.Commands.SendMessage
{
	public sealed record SendMessageToChatCommand : IRequest<SendMessageResponse>
	{
		public Guid ChatId { get; set; }
		public Guid ClientMessageId { get; set; }
		public Guid SenderId { get; set; }
		public string Text { get; set; } = string.Empty;
	}
}
