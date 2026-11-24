using Auth.Application.Security;
using Auth.Application.Services;
using Auth.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Auth
{
	internal sealed class Program
	{
		internal static void Main (string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContextPool<AppDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

			builder.Services.AddSingleton<PasswordHasher>();
			builder.Services.AddSingleton<TokenProvider>();
			builder.Services.AddSingleton<JwtConfiguration>();

			builder.Services.AddScoped<AuthService>();

			//builder.Services.AddGrpc();

			WebApplication app = builder.Build();

			if ( app.Environment.IsDevelopment() )
			{
				using IServiceScope scope = app.Services.CreateScope();
				AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				context.Database.Migrate();
			}

			//app.MapGrpcService<AuthGrpcService>();

			app.Run();
		}
	}
}
