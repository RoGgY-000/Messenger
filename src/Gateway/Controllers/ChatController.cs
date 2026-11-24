using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Chat.Commands.SendMessage;
using Shared.DTO.Chat.Queries.GetUnreadMessages;
using Shared.Extensions;

namespace Gateway.Controllers
{
	[ApiController]
	[Route("api/chat")]
	public sealed class ChatController
		(ISender mediatr)
		: ControllerBase
	{
		[HttpGet("unread")]
		public async Task<IActionResult> GetUnread ()
		{
			
			if ( !User.TryParseUserId(out Guid userId) )
			{
				return Unauthorized();
			}
			GetUnreadMessagesQuery request = new(userId);
			GetUnreadMessageResponse[] result = await mediatr.Send(request);
			return Ok(result);
		}

		[HttpPost("send_message_to_chat")]
		public async Task<IActionResult> SendMessageToChat (SendMessageToChatCommand request)
		{
			if ( !User.TryParseUserId(out Guid userId) || request.SenderId != userId)
			{
				return Unauthorized();
			}
			SendMessageResponse result = await mediatr.Send(request);
			return Ok(result);
		}

		[HttpPost("send_message_to_user")]
		public async Task<IActionResult> SendMessageToUser (SendMessageToUserCommand request)
		{
			if ( !User.TryParseUserId(out Guid userId) || request.SenderId != userId )
			{
				return Unauthorized();
			}
			SendMessageResponse result = await mediatr.Send(request);
			return Ok(result);
		}
	}
}