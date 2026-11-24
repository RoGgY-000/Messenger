using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Shared.DTO.Chat.Queries.GetChatsIdsByUserId
{
	public sealed record GetUserChatsWithLastMessageIdQuery : IRequest<GetUserChatWithLastMessageIdResponse[]>
	{
		public Guid UserId { get; set; }
	}
}
