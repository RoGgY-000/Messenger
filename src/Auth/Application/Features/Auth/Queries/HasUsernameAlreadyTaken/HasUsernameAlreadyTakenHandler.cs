using AuthService.Infrastructure;
using MediatR;
using Shared.DTO.Auth.Queries.HasUsernameAlreadyTaken;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Features.Auth.Queries.HasUsernameAlreadyTaken
{
	internal sealed class HasUsernameAlreadyTakenHandler
		(AuthDbContext context)
		: IRequestHandler<HasUsernameAlreadyTakenRequest, HasUsernameAlreadyTakenResponse>
	{
		public async Task<HasUsernameAlreadyTakenResponse> Handle (HasUsernameAlreadyTakenRequest request, CancellationToken cancellationToken)
		{
			bool hasTaken = await context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
			return new()
			{
				Username = request.Username,
				HasTaken = hasTaken
			};
		}
	}
}
