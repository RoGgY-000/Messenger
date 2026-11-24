using ChatService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configurations
{
	internal sealed class ChatConfiguration : IEntityTypeConfiguration<Chat>
	{
		public void Configure (EntityTypeBuilder<Chat> builder)
		{
			builder.ToTable("Chats");
			builder.HasKey(c => c.Id);

			builder.Property(c => c.Name)
				.HasMaxLength(256)
				.IsRequired();
		}
	}
}
