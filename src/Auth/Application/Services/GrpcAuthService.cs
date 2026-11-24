using Grpc.Core;
using Mapster;
using MediatR;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Commands;
using Shared.DTO.Auth.Commands.Refresh;
using Shared.DTO.Auth.Commands.SignIn;
using Shared.DTO.Auth.Commands.SignUp;
using Shared.DTO.Auth.Queries.GetUsersById;
using Shared.DTO.Auth.Queries.HasEmailAlreadyTaken;
using Shared.DTO.Auth.Queries.HasPhoneNumberAlreadyTaken;
using Shared.DTO.Auth.Queries.HasUsernameAlreadyTaken;
using Shared.DTO.Auth.Queries.SearchUserById;
using Shared.DTO.Auth.Queries.SearchUserByUsername;
using Shared.DTO.Auth.Queries.VerifyPassword;

namespace AuthService.Application.Services
{
	public sealed class GrpcAuthService
		(ISender mediatr)
		: AuthServiceGrpc.AuthServiceGrpcBase
	{
		public override async Task<GetUsersByIdGrpcResponse> GetUsersById (GetUsersByIdGrpcRequest grpcRequest, ServerCallContext context)
		{
			GetUsersByIdQuery request = new()
			{
				Users = grpcRequest.Users.Select(id => Guid.Parse(id)).ToArray()
			};
			GetUserByIdResponse[] response = await mediatr.Send(request, context.CancellationToken);
			GetUsersByIdGrpcResponse grpcResponse = new();
			grpcResponse.Users.AddRange(response.Adapt<UserGrpcResponse[]>());
			return grpcResponse;
		}

		public override async Task<HasUsernameAlreadyTakenGrpcResponse> HasUsernameAlreadyTaken (HasUsernameAlreadyTakenGrpcRequest grpcRequest, ServerCallContext context)
		{
			HasUsernameAlreadyTakenRequest request = grpcRequest.Adapt<HasUsernameAlreadyTakenRequest>();
			HasUsernameAlreadyTakenResponse response = await mediatr.Send(request, context.CancellationToken);
			return response.Adapt<HasUsernameAlreadyTakenGrpcResponse>();
		}

		public override async Task<HasEmailAlreadyTakenGrpcResponse> HasEmailAlreadyTaken (HasEmailAlreadyTakenGrpcRequest grpcRequest, ServerCallContext context)
		{
			HasEmailAlreadyTakenRequest request = grpcRequest.Adapt<HasEmailAlreadyTakenRequest>();
			HasEmailAlreadyTakenResponse response = await mediatr.Send(request, context.CancellationToken);
			return response.Adapt<HasEmailAlreadyTakenGrpcResponse>();
		}

		public override async Task<HasPhoneNumberAlreadyTakenGrpcResponse> HasPhoneNumberAlreadyTaken (HasPhoneNumberAlreadyTakenGrpcRequest grpcRequest, ServerCallContext context)
		{
			HasPhoneNumberAlreadyTakenRequest request = grpcRequest.Adapt<HasPhoneNumberAlreadyTakenRequest>();
			HasPhoneNumberAlreadyTakenResponse response = await mediatr.Send(request, context.CancellationToken);
			return response.Adapt<HasPhoneNumberAlreadyTakenGrpcResponse>();
		}

		public override async Task<AuthGrpcResponse> Refresh (RefreshGrpcRequest grpcRequest, ServerCallContext context)
		{
			RefreshCommand request = new()
			{
				RefreshToken = Guid.Parse(grpcRequest.RefreshToken),
				UserId = Guid.Parse(grpcRequest.UserId)
			};
			AuthResponse response = await mediatr.Send(request, context.CancellationToken);
			return new()
			{
				AccessToken = response.AccessToken,
				ExpiresAt = response.ExpiresAt.ToString("o"),
				RefreshToken = response.RefreshToken,
				UserId = response.UserId.ToString()
			};
		}

		public override async Task<AuthGrpcResponse> SignIn (SignInGrpcRequest grpcRequest, ServerCallContext context)
		{
			SignInCommand request = grpcRequest.Adapt<SignInCommand>();
			AuthResponse response = await mediatr.Send(request, context.CancellationToken);
			return new()
			{
				UserId = response.UserId.ToString(),
				ExpiresAt = response.ExpiresAt.ToString("o"),
				AccessToken = response.AccessToken,
				RefreshToken = response.RefreshToken
			};
		}

		public override async Task<AuthGrpcResponse> SignUp (SignUpGrpcRequest grpcRequest, ServerCallContext context)
		{
			SignUpCommand request = grpcRequest.Adapt<SignUpCommand>();
			AuthResponse response = await mediatr.Send(request, context.CancellationToken);
			return new()
			{
				UserId = response.UserId.ToString(),
				ExpiresAt = response.ExpiresAt.ToString("o"),
				AccessToken = response.AccessToken,
				RefreshToken = response.RefreshToken
			};
		}

		public override async Task<SearchUserByIdGrpcResponse> SearchUserById (SearchUserByIdGrpcRequest grpcRequest, ServerCallContext context)
		{
			SearchUserByIdQuery request = grpcRequest.Adapt<SearchUserByIdQuery>();
			SearchUserByIdResponse response = await mediatr.Send(request, context.CancellationToken);
			SearchUserByIdGrpcResponse grpcResponse = response.Adapt<SearchUserByIdGrpcResponse>();
			return grpcResponse;
		}

		public override async Task<SearchUsersByUsernameGrpcResponse> SearchUsersByUsername (SearchUsersByUsernameGrpcRequest grpcRequest, ServerCallContext context)
		{
			SearchUsersByUsernameQuery request = grpcRequest.Adapt<SearchUsersByUsernameQuery>();
			SearchUserByUsernameResponse[] response = await mediatr.Send(request);
			SearchUsersByUsernameGrpcResponse grpcResponse = new();
			grpcResponse.Users.AddRange(response.Adapt<SearchUserByUsernameGrpcResponse[]>());
			return grpcResponse;
		}

		public override async Task<VerifyPasswordGrpcResponse> VerifyPassword (VerifyPasswordGrpcRequest grpcRequest, ServerCallContext context)
		{
			VerifyPasswordRequest verifyPasswordRequest = grpcRequest.Adapt<VerifyPasswordRequest>();
			VerifyPasswordResponse verifyPasswordResponse = await mediatr.Send(verifyPasswordRequest, context.CancellationToken);
			return verifyPasswordResponse.Adapt<VerifyPasswordGrpcResponse>();
		}
	}
}
