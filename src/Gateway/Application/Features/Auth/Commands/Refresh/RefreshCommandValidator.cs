using FluentValidation;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Commands.Refresh;

namespace GatewayService.Application.Features.Auth.Commands.Refresh
{
	public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
	{
		private readonly AuthServiceGrpc.AuthServiceGrpcClient _grpcClient;
		public RefreshCommandValidator (AuthServiceGrpc.AuthServiceGrpcClient grpcClient)
		{
			_grpcClient = grpcClient;

			RuleFor(c => c.RefreshToken)
				.Must(rt => rt != Guid.Empty)
				.WithMessage("Refresh token is incorrect");

			RuleFor(c => c.UserId)
				.Must(ui => ui != Guid.Empty)
				.WithMessage("User id is incorrect")
				.MustAsync(SendSearchUserById)
				.WithMessage("User with this Id does not exist");
		}

		private async Task<bool> SendSearchUserById (Guid userId, CancellationToken cancellationToken)
		{
			SearchUserByIdGrpcRequest grpcRequest = new()
			{
				UserId = userId.ToString()
			};
			SearchUserByIdGrpcResponse grpcResponse = await _grpcClient.SearchUserByIdAsync(grpcRequest, cancellationToken: cancellationToken);
			return grpcResponse.IsExists;
		}
	}
}
