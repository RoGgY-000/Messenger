using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure
{
	internal sealed class AuthDbContext
		(DbContextOptions<AuthDbContext> options,
		IConfiguration configuration)
		: DbContext(options)
	{
		internal DbSet<User> Users => Set<User>();
		internal DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

		protected override void OnModelCreating (ModelBuilder modelBuilder)
			=> modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);

		protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
		{
			if ( !optionsBuilder.IsConfigured )
			{
				string connectionString = configuration.GetConnectionString("DbConnection")
					?? throw new Exception("Db connection string not found");
				optionsBuilder
					.UseNpgsql(connectionString)
					.EnableDetailedErrors()
					.EnableSensitiveDataLogging();
			}
		}
	}
}
