using System.Net;

using deli.api.Extensions;

namespace deli.api.Handlers
{
	public abstract class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		public abstract (HttpStatusCode code, ErrorType errorType, string message) GetResponse(Exception exception);

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception e)
			{
				// тут нужно добавить запись в лог ...
				//Logger.Error(e, context.Request.Path.StringId);

				var (statusCode, errorType, message) = GetResponse(e);
				await context.Response.WriteResponseAsync(new Error(errorType, message, "ExceptionHandlerMiddleware",  e), statusCode);
			}
		}
	}

}
