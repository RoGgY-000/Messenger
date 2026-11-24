
namespace AuthService.Domain
{
	public sealed class User
	{
		public Guid Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; }
		public ICollection<RefreshToken> Tokens { get; set; } = [];
		public string Username { get; set; } = string.Empty;
	}
}
