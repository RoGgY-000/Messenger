using ChatService.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Infrastructure
{
	internal sealed class ChatDbContext
		(DbContextOptions<ChatDbContext> options)
		: DbContext(options)
	{
		public DbSet<Message> Messages => Set<Message>();
		public DbSet<Chat> Chats => Set<Chat>();
		public DbSet<ChatMember> ChatMembers => Set<ChatMember>();

		protected override void OnModelCreating (ModelBuilder modelBuilder)
			=> modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);
	}
}