namespace AuthService.Application.Security
{
	public sealed class JwtConfiguration (IConfiguration configuration)
	{
		internal TimeSpan AuthLifeTime { get; } = TimeSpan.FromDays(30);
		internal TimeSpan TokenLifeTime { get; } = TimeSpan.FromMinutes(15);
		internal readonly string Issuer = "AuthService";
		internal readonly string Audience = "GatewayService";
		internal string SecretKey { get; private set; } = configuration["JwtSettings:SecretKey"] ?? throw new Exception("JWT secret key not found!");
	}
}
