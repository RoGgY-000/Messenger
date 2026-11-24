namespace Shared.DTO.Auth.Queries.SearchUserById
{
	public sealed record SearchUserByIdResponse
	{
		public Guid UserId { get; set; }
		public bool IsExists { get; set; }
	}
}
