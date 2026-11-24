using FluentValidation;
using Shared.DTO.Auth.Queries.SearchUserByUsername;

namespace Gateway.Application.Features.Auth.Queries.SearchUser
{
	public class SearchUsersByUsernameQueryValidator 
		: AbstractValidator<SearchUsersByUsernameQuery>
	{
		public SearchUsersByUsernameQueryValidator ()
		{
			RuleFor(q => q.Username)
				.NotEmpty()
				.WithMessage("Username cannot be empty")
				.MinimumLength(4)
				.WithMessage("Username must contain at least 4 characters")
				.MaximumLength(256)
				.WithMessage("Username must contain up to 256 characters");
		}
	}
}
