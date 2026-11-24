using MediatR;

namespace Shared.DTO.Auth.Queries.SearchUserById
{
	public sealed record SearchUserByIdQuery : IRequest<SearchUserByIdResponse>
	{
		public Guid UserId { get; set; }
	}
}
