using System.Text;
using ConnectionService.Consumers;
using ConnectionService.Hubs;
using ConnectionService.Services;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Messenger.Shared.Protos.Auth;
using Messenger.Shared.Protos.Chat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ConnectionService
{
	internal sealed class Program
	{
		public static void Main (string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			builder.Services.AddGrpc();

			builder.Services.AddMassTransit(c =>
			{
				c.AddConsumer<MessageSentConsumer>();
				c.UsingRabbitMq((context, configurator) =>
				{
					string connection = builder.Configuration.GetConnectionString("RabbitMQ")
					?? throw new Exception("RabbitMQ configuration string not found");

					configurator.Host(connection);
					configurator.ConfigureEndpoints(context);
				});
			});

			builder.Services.AddSignalR();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => options.TokenValidationParameters = new()
				{
					ValidateIssuer = true,
					ValidIssuer = "AuthService",

					ValidateAudience = true,
					ValidAudience = "GatewayService",

					ValidateLifetime = true,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes("super_secret_key_messenger_app_2026_long_enough"))
				});

			builder.Services.AddGrpcClient<ChatServiceGrpc.ChatServiceGrpcClient>()
				.ConfigureHttpClient(client
				=> client.BaseAddress = new Uri(
					builder.Configuration["Services:Chat"] ?? throw new Exception("Http Address fot Chat service not found")));

			builder.Services.AddGrpcClient<AuthServiceGrpc.AuthServiceGrpcClient>()
				.ConfigureHttpClient(client
				=> client.BaseAddress = new Uri(
					builder.Configuration["Services:Auth"] ?? throw new Exception("Http Address fot Auth service not found")));

			WebApplication app = builder.Build();
			app.MapHub<ChatHub>("/hubs/chat");
			app.MapGrpcService<GrpcConnectionService>();
			app.Run();
		}
	}
}
