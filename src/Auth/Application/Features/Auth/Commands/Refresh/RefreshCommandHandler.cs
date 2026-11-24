using AuthService.Application.Security;
using AuthService.Domain;
using AuthService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Shared.DTO.Auth.Commands.Refresh;
using Shared.DTO.Auth.Commands;

namespace AuthService.Application.Features.Auth.Commands.Refresh
{
	internal class RefreshCommandHandler
		(AuthDbContext context,
		JwtConfiguration jwtConfiguration,
		TokenProvider tokenProvider)
		: IRequestHandler<RefreshCommand, AuthResponse>
	{ // добавить реакцию на просроченный токен
		public async Task<AuthResponse> Handle (RefreshCommand request, CancellationToken cancellationToken)
		{
			RefreshToken[] userTokens = await context.RefreshTokens
				.Where(rt => rt.UserId == request.UserId)
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

			RefreshToken newToken = new()
			{
				ExpiresAt = DateTime.UtcNow + jwtConfiguration.AuthLifeTime,
				Id = Guid.CreateVersion7(),
				IsRevoked = false,
				Token = Guid.CreateVersion7(),
				UserId = request.UserId
			};
			context.RefreshTokens.Add(newToken);
			await context.SaveChangesAsync(cancellationToken);
			return new()
			{
				AccessToken = tokenProvider.CreateDefault(request.UserId, DateTime.UtcNow + jwtConfiguration.TokenLifeTime),
				ExpiresAt = newToken.ExpiresAt,
				RefreshToken = newToken.Token.ToString(),
				UserId= request.UserId
			};
		}
	}
}
