using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.Refresh;
using Shared.DTO.Auth.Commands.SignIn;
using Shared.DTO.Auth.Commands.SignUp;
using Shared.DTO.Auth.Queries.SearchUserByUsername;
using Shared.Extensions;

namespace Gateway.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public sealed class AuthController 
		(ISender mediatr): ControllerBase
	{
		[AllowAnonymous]
		[HttpPost("signup")]
		public async Task<IActionResult> SignUp (SignUpCommand request)
		{
			AuthResponse result = await mediatr.Send(request);
			return Ok(result);
		}

		[AllowAnonymous]
		[HttpPost("signin")]
		public async Task<IActionResult> SignIn (SignInCommand request)
		{
			AuthResponse result = await mediatr.Send(request);
			return Ok(result);
		}

		[Authorize(AuthenticationSchemes = "ExpiredJwtScheme")]
		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh ([FromBody] Guid refreshToken)
		{
			if ( !User.TryParseUserId(out Guid userId))
			{
				return Unauthorized();
			}
			RefreshCommand request = new()
			{
				RefreshToken = refreshToken,
				UserId = userId
			};
			AuthResponse result = await mediatr.Send(request);
			return Ok(result);
		}

		[HttpGet("users/search/{username}")]
		public async Task<IActionResult> SearchUserByUsername (string username)
		{
			SearchUsersByUsernameQuery query = new(username);
			SearchUserByUsernameResponse[] result = await mediatr.Send(query);
			return Ok(result);
		}
	}
}
