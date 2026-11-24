using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Konscious.Security.Cryptography;

namespace AuthService.Application.Security
{
	public sealed class PasswordHasher
	{
		internal string Hash (string password)
		{
			ArgumentNullException.ThrowIfNullOrWhiteSpace(password);

			byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
			byte[] salt = RandomNumberGenerator.GetBytes(24);
			byte[] hashBytes = Hash(passwordBytes, salt);
			string hash = $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashBytes)}";
			return hash;
		}

		internal bool Verify (string password, string hash)
		{
			ArgumentNullException.ThrowIfNullOrWhiteSpace(password);
			ArgumentNullException.ThrowIfNullOrWhiteSpace(hash);

			if ( hash.Length != 81 || hash[32] != '.' )
			{
				throw new ArgumentException("Hash string is incorrect");
			}

			byte[] saltBytes = new byte[24], expectedHashBytes = new byte[36];
			ReadOnlySpan<char> saltString = hash.AsSpan(0, 32);
			ReadOnlySpan<char> expectedHashString = hash.AsSpan(33);
			byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
			if ( !Convert.TryFromBase64Chars(saltString, saltBytes, out int writtenSalt) || writtenSalt != saltBytes.Length )
			{
				throw new CryptographicException("Base64 string was not converted successfully");
			}
			if ( !Convert.TryFromBase64Chars(expectedHashString, expectedHashBytes, out int writtenHash) || writtenHash != expectedHashBytes.Length )
			{
				throw new CryptographicException("Base64 hash was not converted successfully");
			}

			byte[] calculatedHash = Hash(passwordBytes, saltBytes);
			return expectedHashBytes.SequenceEqual(calculatedHash);
		}

		private byte[] Hash (byte[] password, byte[] salt)
		{
			using Argon2id argon = new(password)
			{
				DegreeOfParallelism = 2,
				MemorySize = 32 * 1024,
				Iterations = 2,
				Salt = salt
			};
			byte[] hashBytes = argon.GetBytes(36);
			return hashBytes;
		}
	}
}
