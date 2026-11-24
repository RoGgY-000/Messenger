using AuthService.Domain;
using AuthService.Infrastructure;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Auth.Queries.SearchUserByUsername;

namespace AuthService.Application.Features.Auth.Queries.SearchUserByUsername
{
	internal sealed class SearchUsersByUsernameQueryHandler
		(AuthDbContext context)
		: IRequestHandler<SearchUsersByUsernameQuery, SearchUserByUsernameResponse[]>
	{
		public async Task<SearchUserByUsernameResponse[]> Handle (SearchUsersByUsernameQuery request, CancellationToken cancellationToken)
		{
			if ( string.IsNullOrWhiteSpace(request.Username) )
			{
				return [];
			}

			string query = request.Username.Trim();

			return await context.Users
				.Where(u => EF.Functions.ILike(u.Username, $"{query}%")
				|| EF.Functions.ILike(u.Name, $"{query}%"))
				.Take(10)
				.Select( u => new SearchUserByUsernameResponse()
				{
					Id = u.Id,
					Name = u.Name,
					Username = u.Username
				})
				.ToArrayAsync(cancellationToken);
		}
	}
}
