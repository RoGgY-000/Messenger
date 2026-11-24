using AuthService.Application.Security;
using AuthService.Domain;
using AuthService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.SignIn;

namespace AuthService.Application.Features.Auth.Commands.SignIn
{
	internal sealed class SignInCommandHandler
		(AuthDbContext context,
		TokenProvider tokenProvider,
		JwtConfiguration jwtConfiguration)
		: IRequestHandler<SignInCommand, AuthResponse>
	{
		public async Task<AuthResponse> Handle (SignInCommand request, CancellationToken cancellationToken)
		{
			User user = await context.Users.SingleAsync(u => u.Username == request.Username, cancellationToken);

			RefreshToken[] userTokens = await context.RefreshTokens
				.Where(rt => rt.UserId == user.Id)
				.OrderBy(rt => rt.Id)
				.ToArrayAsync(cancellationToken);

			if ( userTokens.Length > 0 )
			{
				foreach ( RefreshToken token in userTokens )
				{
					token.IsRevoked = true;
				}

				if ( userTokens.Length >= 5 )
				{
					context.RefreshTokens.RemoveRange(userTokens[..(userTokens.Length - 4)]);
				}
			}

			RefreshToken refreshToken = new()
			{
				Id = Guid.CreateVersion7(),
				ExpiresAt = DateTime.UtcNow + jwtConfiguration.AuthLifeTime,
				Token = Guid.CreateVersion7(),
				UserId = user.Id,
				User = user
			};
			context.RefreshTokens.Add(refreshToken);
			await context.SaveChangesAsync(cancellationToken);
			string accessToken = tokenProvider.CreateDefault(user.Id, DateTime.UtcNow + jwtConfiguration.TokenLifeTime);
			AuthResponse result = new()
			{
				AccessToken = accessToken,
				ExpiresAt = refreshToken.ExpiresAt,
				RefreshToken = refreshToken.Token.ToString(),
				UserId = user.Id
			};
			return result;
		}
	}
}