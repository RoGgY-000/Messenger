namespace Shared.DTO.Auth.Queries.VerifyPassword
{
	public record VerifyPasswordResponse
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public bool Verified { get; set; } = false;
	}
}
