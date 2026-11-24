using ChatService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configurations
{
	internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
	{
		public void Configure (EntityTypeBuilder<Message> builder)
		{
			builder.ToTable("Messages");
			builder.HasKey(m => m.Id);
			builder.HasIndex(m => m.ChatId);

			builder.Property(m => m.ChatId)
				.IsRequired();

			builder.Property(m => m.SenderId)
				.IsRequired();

			builder.Property(m => m.SentAt)
				.IsRequired();

			builder.Property(m => m.Text)
				.HasMaxLength(4096)
				.IsRequired();
		}
	}
}
