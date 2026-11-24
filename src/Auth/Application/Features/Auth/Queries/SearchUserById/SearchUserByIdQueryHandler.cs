using AuthService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Auth.Queries.SearchUserById;

namespace AuthService.Application.Features.Auth.Queries.SearchUserById
{
	internal class SearchUserByIdQueryHandler
		(AuthDbContext context)
		: IRequestHandler<SearchUserByIdQuery, SearchUserByIdResponse>
	{
		public async Task<SearchUserByIdResponse> Handle (SearchUserByIdQuery request, CancellationToken cancellationToken)
		{
			bool exists = await context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
			SearchUserByIdResponse response = new()
			{
				UserId = request.UserId,
				IsExists = exists
			};
			return response;
		}
	}
}
