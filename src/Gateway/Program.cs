using System.Text;
using FluentValidation;
using Gateway.Middlewares;
using GatewayService.Application.Features.PipelineBehaviors;
using Messenger.Shared.Protos.Auth;
using Messenger.Shared.Protos.Chat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

namespace Gateway
{
	internal sealed class Program
	{
		internal static void Main (string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			
			ConfigureBuilder(builder);

			WebApplication app = builder.Build();

			ConfigureApp(app);

			app.Run();
		}

		private static void ConfigureBuilder (WebApplicationBuilder builder)
		{
			builder.Services.AddReverseProxy()
				.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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
				})
				.AddJwtBearer("ExpiredJwtScheme", options => options.TokenValidationParameters = new()
				{
					ValidateIssuer = true,
					ValidIssuer = "AuthService",

					ValidateAudience = true,
					ValidAudience = "GatewayService",

					ValidateLifetime = false,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes("super_secret_key_messenger_app_2026_long_enough"))
				});

			builder.Services.AddExceptionHandler<GlobalExceptionHandlerWiddleware>();
			builder.Services.AddProblemDetails();

			builder.Services.AddControllers();

			builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
			ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
			ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

			builder.Services.AddMediatR(conf =>
			{
				conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
				conf.RegisterServicesFromAssembly(typeof(Program).Assembly);
			});

			builder.Services.AddGrpcClient<AuthServiceGrpc.AuthServiceGrpcClient>()
				.ConfigureHttpClient(client
				=> client.BaseAddress = new Uri(
					builder.Configuration["Services:Auth"] ?? throw new Exception("Http Address fot Auth service not found")));

			builder.Services.AddGrpcClient<ChatServiceGrpc.ChatServiceGrpcClient>()
				.ConfigureHttpClient(client
				=> client.BaseAddress = new Uri(
					builder.Configuration["Services:Chat"] ?? throw new Exception("Http Address fot Chat service not found")));

			if ( builder.Environment.IsDevelopment() )
			{
				builder.Services.AddOpenApi();
			}

			builder.Services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));
		}

		private static void ConfigureApp (WebApplication app)
		{
			app.UseExceptionHandler();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapReverseProxy();
			app.MapControllers();

			if ( app.Environment.IsDevelopment() )
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
			}
		}
	}
}