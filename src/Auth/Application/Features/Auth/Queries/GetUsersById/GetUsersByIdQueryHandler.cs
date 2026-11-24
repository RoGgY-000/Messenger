using AuthService.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Shared.DTO.Auth.Queries.GetUsersById;

namespace AuthService.Application.Features.Auth.Queries.GetUsersById
{
	internal class GetUsersByIdQueryHandler
		(AuthDbContext context)
		: IRequestHandler<GetUsersByIdQuery, GetUserByIdResponse[]>
	{
		public async Task<GetUserByIdResponse[]> Handle (GetUsersByIdQuery request, CancellationToken cancellationToken)
			=> await context.Users.Where(u => request.Users.Contains(u.Id)).ProjectToType<GetUserByIdResponse>().ToArrayAsync(cancellationToken);
	}
}
