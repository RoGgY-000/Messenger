using MediatR;

namespace Shared.DTO.Chat.Commands.SendMessage
{
	public sealed record SendMessageToUserCommand : IRequest<SendMessageResponse>
	{
		public Guid ClientMessageId { get; set; }
		public Guid RecipientId { get; set; }
		public Guid SenderId { get; set; }
		public string Text { get; set; } = string.Empty;
	}
}
