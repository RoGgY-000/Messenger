using MediatR;

namespace Shared.DTO.Auth.Queries.GetUsersById
{
	public record GetUsersByIdQuery : IRequest<GetUserByIdResponse[]>
	{
		public Guid[] Users { get; set; } = [];
	}
}
