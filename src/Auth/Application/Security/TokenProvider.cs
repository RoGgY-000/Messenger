using System.Security.Claims;
using System.Text;
using AuthService.Domain;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Application.Security
{
	public sealed class TokenProvider (JwtConfiguration jwtConfiguration)
	{
		public string CreateDefault (Guid userId, DateTime expires)
		{
			KeyValuePair<string, string> userIdClaim = new(ClaimTypes.NameIdentifier, userId.ToString());
			return Create(expires, userIdClaim);
		}
		private string Create (DateTime expires, params KeyValuePair<string, string>[] claims)
		{
			string secretKey = jwtConfiguration.SecretKey;
			SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));
			SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(),
				Expires = expires,
				Issuer = jwtConfiguration.Issuer,
				Audience = jwtConfiguration.Audience,
				SigningCredentials = credentials
			};
			for ( int i = 0; i < claims.Length; i++ )
			{
				Claim claim = new(claims[i].Key, claims[i].Value);
				tokenDescriptor.Subject.AddClaim(claim);
			}
			JsonWebTokenHandler handler = new();
			string token = handler.CreateToken(tokenDescriptor);
			return token;
		}
	}
}
