using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace GatewayService.Application.Features.PipelineBehaviors
{
	public sealed class ValidationBehavior<TRequest, TResponse>
		(IValidator<TRequest> validator)
		: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
	{
		public async Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			await validator.ValidateAndThrowAsync(request, cancellationToken);
			return await next(cancellationToken);
		}
	}
}
