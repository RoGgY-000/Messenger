namespace AuthService.Domain
{
	public sealed class RefreshToken
	{
		public Guid Id { get; set; }
		public DateTime ExpiresAt { get; set; }
		public bool IsRevoked { get; set; } = false;
		public Guid Token { get; set; }
		public Guid UserId { get; set; }
		public User? User { get; set; }
	}
}
