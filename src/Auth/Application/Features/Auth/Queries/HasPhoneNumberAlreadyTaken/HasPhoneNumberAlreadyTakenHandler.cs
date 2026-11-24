using AuthService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Auth.Queries.HasPhoneNumberAlreadyTaken;

namespace AuthService.Application.Features.Auth.Queries.HasPhoneNumberAlreadyTaken
{
	internal sealed class HasPhoneNumberAlreadyTakenHandler
		(AuthDbContext context)
		: IRequestHandler<HasPhoneNumberAlreadyTakenRequest, HasPhoneNumberAlreadyTakenResponse>
	{
		public async Task<HasPhoneNumberAlreadyTakenResponse> Handle (HasPhoneNumberAlreadyTakenRequest request, CancellationToken cancellationToken)
		{
			bool hasTaken = await context.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
			return new()
			{
				PhoneNumber = request.PhoneNumber,
				HasTaken = hasTaken
			};
		}
	}
}
