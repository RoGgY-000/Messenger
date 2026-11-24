using ChatService.Application.Services;
using ChatService.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChatService
{
	internal sealed class Program
	{
		internal static void Main (string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContextPool<ChatDbContext>(optionsBuilder => optionsBuilder
					.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"))
					.EnableDetailedErrors()
					.EnableSensitiveDataLogging());

			builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());
			builder.Services.AddMassTransit( c => c.UsingRabbitMq( (context, configurator) =>
			{
				string connection = builder.Configuration.GetConnectionString("RabbitMQ")
				?? throw new Exception("RabbitMQ configuration string not found");

				configurator.Host(connection);
				configurator.ConfigureEndpoints(context);
			}));
			builder.Services.AddGrpc();

			WebApplication app = builder.Build();

			if ( app.Environment.IsDevelopment() )
			{
				using ChatDbContext DbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<ChatDbContext>();
				DbContext.Database.Migrate();
			}

			app.MapGrpcService<GrpcChatService>();

			app.Run();
		}
	}
}