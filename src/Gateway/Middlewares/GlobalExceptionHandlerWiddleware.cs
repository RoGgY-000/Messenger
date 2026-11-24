using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Middlewares
{
	public class GlobalExceptionHandlerWiddleware : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync (HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			HttpStatusCode code = HttpStatusCode.InternalServerError;
			object result = string.Empty;
			switch (exception)
			{
				case ArgumentException or InvalidOperationException:
					code = HttpStatusCode.BadRequest;
					break;
				case ValidationException validationException:
					code = HttpStatusCode.BadRequest;
					Dictionary<string, string[]> errors = validationException.Errors
					   .GroupBy(e => e.PropertyName)
					   .ToDictionary(
						   g => g.Key,
						   g => g.Select(e => e.ErrorMessage).ToArray()
					   );
					ValidationProblemDetails problemDetails = new(errors)
					{
						Status = (int) HttpStatusCode.BadRequest,
						Title = "One or more validation errors occurred."
					};
					result = problemDetails;
					break;
				default:
					break;
			}

			if ( code != HttpStatusCode.InternalServerError )
			{
				httpContext.Response.ContentType = "application/json";
				httpContext.Response.StatusCode = (int) code;
				await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
			}
			return true;
		}
	}
}
