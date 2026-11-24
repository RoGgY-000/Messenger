using FluentValidation;
using Mapster;
using MediatR;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.SignUp;

namespace Gateway.Application.Features.Auth.Commands.SignUp
{
	public class SignUpCommandHandler
		(AuthServiceGrpc.AuthServiceGrpcClient grpcClient,
		IValidator<SignUpCommand> validator)
		: IRequestHandler<SignUpCommand, AuthResponse>
	{
		public async Task<AuthResponse> Handle (SignUpCommand request, CancellationToken cancellationToken)
		{
			await validator.ValidateAndThrowAsync(request, cancellationToken);
			SignUpGrpcRequest grpcRequest = request.Adapt<SignUpGrpcRequest>();
			AuthGrpcResponse grpcResponse = await grpcClient.SignUpAsync(grpcRequest, cancellationToken: cancellationToken);
			Guid userId = Guid.Parse(grpcResponse.UserId);
			DateTime expiresAt = DateTime.Parse(grpcResponse.ExpiresAt);

			AuthResponse result = new()
			{
				AccessToken = grpcResponse.AccessToken,
				UserId = userId,
				ExpiresAt = expiresAt,
				RefreshToken = grpcResponse.RefreshToken
			};

			return result;
		}
	}
}
