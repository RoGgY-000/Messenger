using AuthService.Infrastructure;
using MediatR;
using Shared.DTO.Auth.Queries.HasEmailAlreadyTaken;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Features.Auth.Queries.HasEmailAlreadyTaken
{
	internal sealed class HasEmailAlreadyTakenHandler 
		(AuthDbContext context)
		: IRequestHandler<HasEmailAlreadyTakenRequest, HasEmailAlreadyTakenResponse>
	{
		public async Task<HasEmailAlreadyTakenResponse> Handle (HasEmailAlreadyTakenRequest request, CancellationToken cancellationToken)
		{
			bool hasTaken = await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
			return new()
			{
				Email = request.Email,
				HasTaken = hasTaken
			};
		}
	}
}
