using AuthService.Application.Security;
using AuthService.Domain;
using AuthService.Infrastructure;
using Mapster;
using MassTransit;
using MediatR;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.SignUp;
using Shared.DTO.Auth.Events.UserSignedUp;

namespace AuthService.Application.Features.Auth.Commands.SignUp
{
	internal sealed class SignUpCommandHandler 
		(AuthDbContext context,
		PasswordHasher passwordHasher, 
		TokenProvider tokenProvider,
		JwtConfiguration jwtConfiguration,
		IPublishEndpoint publishEndpoint)
		: IRequestHandler<SignUpCommand, AuthResponse>
	{
		public async Task<AuthResponse> Handle (SignUpCommand request, CancellationToken cancellationToken)
		{
			User user = request.Adapt<User>();

			user.Id = Guid.CreateVersion7();
			user.PasswordHash = passwordHasher.Hash(request.Password);
			DateTime expires = DateTime.UtcNow + jwtConfiguration.TokenLifeTime;
			string token = tokenProvider.CreateDefault(user.Id, expires);
			RefreshToken refreshToken = new()
			{
				Id = Guid.CreateVersion7(),
				ExpiresAt = DateTime.UtcNow + jwtConfiguration.AuthLifeTime,
				Token = Guid.CreateVersion7(),
				UserId = user.Id,
				User = user
			};
			context.Users.Add(user);
			context.RefreshTokens.Add(refreshToken);
			await context.SaveChangesAsync(cancellationToken);

			UserSignedUpEvent e = user.Adapt<UserSignedUpEvent>();
			await publishEndpoint.Publish(e, cancellationToken);

			AuthResponse result = new()
			{
				UserId = user.Id,
				AccessToken = token,
				RefreshToken = refreshToken.Token.ToString(),
				ExpiresAt = expires
			};
			return result;
		}
	}
}
