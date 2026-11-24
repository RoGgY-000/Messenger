using Mapster;
using MediatR;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.SignIn;

namespace Gateway.Application.Features.Auth.Commands.SignIn
{
	public class SignInCommandHandler
		(AuthServiceGrpc.AuthServiceGrpcClient grpcClient)
		: IRequestHandler<SignInCommand, AuthResponse>
	{
		public async Task<AuthResponse> Handle (SignInCommand request, CancellationToken cancellationToken)
		{
			VerifyPasswordGrpcRequest verifyPasswordGrpcRequest = request.Adapt<VerifyPasswordGrpcRequest>();
			VerifyPasswordGrpcResponse verifyPasswordGrpcResponse = await grpcClient.VerifyPasswordAsync(verifyPasswordGrpcRequest, cancellationToken: cancellationToken);
			if ( !verifyPasswordGrpcResponse.Verified )
			{
				throw new ArgumentException("Incorrect password");
			}
			SignInGrpcRequest grpcRequest = request.Adapt<SignInGrpcRequest>();
			AuthGrpcResponse grpcResponse = await grpcClient.SignInAsync(grpcRequest, cancellationToken: cancellationToken);
			Guid userId = Guid.Parse(grpcResponse.UserId);
			DateTime expiresAt = DateTime.Parse(grpcResponse.ExpiresAt);
			return new()
			{
				AccessToken = grpcResponse.AccessToken,
				ExpiresAt = expiresAt,
				RefreshToken = grpcResponse.RefreshToken,
				UserId = userId
			};
		}
	}
}
