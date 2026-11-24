using MediatR;

namespace Shared.DTO.Auth.Queries.SearchUserByUsername
{
	public record SearchUsersByUsernameQuery : IRequest<SearchUserByUsernameResponse[]>
	{
		public string Username { get; set; } = string.Empty;

		public SearchUsersByUsernameQuery (string username) => Username = username;
	}
}
