using Mapster;
using MediatR;
using Messenger.Shared.Protos.Auth;
using Shared.DTO.Auth.Queries.SearchUserByUsername;

namespace Gateway.Application.Features.Auth.Queries.SearchUser
{
	public class SearchUsersByUsernameQueryHandler
		(AuthServiceGrpc.AuthServiceGrpcClient grpcClient)
		: IRequestHandler<SearchUsersByUsernameQuery, SearchUserByUsernameResponse[]>
	{
		public async Task<SearchUserByUsernameResponse[]> Handle (SearchUsersByUsernameQuery request, CancellationToken cancellationToken)
		{
			SearchUsersByUsernameGrpcRequest grpcRequest = request.Adapt<SearchUsersByUsernameGrpcRequest>();
			SearchUsersByUsernameGrpcResponse grpcResponse = await grpcClient.SearchUsersByUsernameAsync(grpcRequest, cancellationToken: cancellationToken);
			SearchUserByUsernameResponse[] response = grpcResponse.Users.Adapt<SearchUserByUsernameResponse[]>();
			return response;
		}
	}
}
