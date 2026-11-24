using Mapster;
using MediatR;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.Refresh;

namespace GatewayService.Application.Features.Auth.Commands.Refresh
{
	internal class RefreshCommandHandler (
		AuthServiceGrpc.AuthServiceGrpcClient grpcClient)
		: IRequestHandler<RefreshCommand, AuthResponse>
	{
		public async Task<AuthResponse> Handle (RefreshCommand request, CancellationToken cancellationToken)
		{
			RefreshGrpcRequest grpcRequest = new()
			{
				RefreshToken = request.RefreshToken.ToString(),
				UserId = request.UserId.ToString(),
			};
			AuthGrpcResponse grpcResponse = await grpcClient.RefreshAsync(grpcRequest, cancellationToken: cancellationToken);
			AuthResponse response = grpcResponse.Adapt<AuthResponse>();
			return response;
		}
	}
}
