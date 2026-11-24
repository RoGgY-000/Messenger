namespace Shared.DTO.Auth.Queries.SearchUserByUsername
{
	public record SearchUserByUsernameResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
	}
}
