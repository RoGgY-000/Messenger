namespace Shared.DTO.Auth.Queries.GetUsersById
{
	public record GetUserByIdResponse
	{
		public Guid Id { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
	}
}