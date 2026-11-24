using ChatService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatService.Infrastructure.Configurations
{
	internal sealed class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
	{
		public void Configure (EntityTypeBuilder<ChatMember> builder)
		{
			builder.ToTable("ChatMembers");
			builder.HasKey(cm => cm.Id);
			builder.HasIndex(cm => cm.UserId);

			builder.Property(cm => cm.ChatId)
				.IsRequired();

			builder.Property(cm => cm.UserId)
				.IsRequired();

			builder.Property(cm => cm.JoinedAt)
				.IsRequired();
		}
	}
}
