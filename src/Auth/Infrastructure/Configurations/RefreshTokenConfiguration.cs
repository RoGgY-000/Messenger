using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AuthService.Infrastructure.Configurations
{
	internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure (EntityTypeBuilder<RefreshToken> builder)
		{
			builder.ToTable("RefreshTokens");
			builder.HasKey(t => t.Id);

			builder.HasIndex(t => t.Token)
				.IsUnique();

			builder.HasOne(t => t.User)
				.WithMany(u => u.Tokens)
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.Property(t => t.ExpiresAt)
				.IsRequired();

			builder.Property(t => t.IsRevoked)
				.IsRequired();

			builder.Property(t => t.Token)
				.IsRequired();
		}
	}
}
