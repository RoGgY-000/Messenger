using AuthService.Application.Security;
using AuthService.Application.Services;
using AuthService.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuthService
{
	internal sealed class Program
	{
		internal static void Main (string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContextPool<AuthDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")).EnableSensitiveDataLogging());
			builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());
			builder.Services.AddGrpc();
			AddServices(builder);
			ConfigureMassTransit(builder);
			WebApplication app = builder.Build();

			if ( app.Environment.IsDevelopment() )
			{
				using IServiceScope scope = app.Services.CreateScope();
				AuthDbContext context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
				context.Database.Migrate();
			}

			app.MapGrpcService<GrpcAuthService>();

			app.Run();
		}

		private static WebApplicationBuilder AddServices (WebApplicationBuilder builder)
		{
			builder.Services.AddSingleton<PasswordHasher>();
			builder.Services.AddSingleton<TokenProvider>();
			builder.Services.AddSingleton<JwtConfiguration>();
			return builder;
		}

		private static WebApplicationBuilder ConfigureMassTransit (WebApplicationBuilder builder)
		{
			builder.Services.AddMassTransit(c => c.UsingRabbitMq((context, configurator) =>
			{
				string connection = builder.Configuration.GetConnectionString("RabbitMQ")
				?? throw new Exception("RabbitMQ configuration string not found");

				configurator.Host(connection);
				configurator.ConfigureEndpoints(context);
			}));
			return builder;
		}
	}
}
