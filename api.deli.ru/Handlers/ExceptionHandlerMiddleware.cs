using System.Net;

namespace api.deli.ru.Handlers
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

				context.Response.ContentType = "application/json";

				var (status, errorTipe, message) = GetResponse(e);
				context.Response.StatusCode = (int)status;

				await context.Response.WriteAsync(Utils.JsonSerializer(new Error(errorTipe, e)));
			}
		}
	}

}
