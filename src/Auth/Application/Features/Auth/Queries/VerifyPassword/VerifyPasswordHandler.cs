using AuthService.Application.Security;
using AuthService.Infrastructure;
using MediatR;
using Shared.DTO.Auth.Queries.VerifyPassword;
using Microsoft.EntityFrameworkCore;
using AuthService.Domain;
namespace AuthService.Application.Features.Auth.Queries
{
	internal sealed class VerifyPasswordHandler
		(AuthDbContext context,
		PasswordHasher passwordHasher)
		: IRequestHandler<VerifyPasswordRequest, VerifyPasswordResponse>
	{
		public async Task<VerifyPasswordResponse> Handle (VerifyPasswordRequest request, CancellationToken cancellationToken)
		{
			User user = await context.Users.Where(u => u.Username == request.Username).AsNoTracking().SingleAsync(cancellationToken);
			bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);
			return new()
			{
				Username = request.Username,
				Password = request.Password,
				Verified = verified
			};
		}
	}
}
