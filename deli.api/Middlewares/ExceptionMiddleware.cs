using System.Net;

using deli.api.Handlers;

namespace deli.api.Middlewares
{
	public class ExceptionMiddleware : ExceptionHandlerMiddleware
	{
		public ExceptionMiddleware(RequestDelegate next) : base(next) { }

		public override (HttpStatusCode code, ErrorType errorType, string message) GetResponse(Exception exception)
		{
			ErrorType type;
			HttpStatusCode code;
			switch (exception)
			{
				case KeyNotFoundException or FileNotFoundException:
					type = ErrorType.NotFound;
					code = HttpStatusCode.NotFound;
					break;
				case UnauthorizedAccessException:
					type = ErrorType.Unauthorized;
					code = HttpStatusCode.Unauthorized;
					break;
				case ArgumentException or InvalidOperationException:
					type = ErrorType.BadRequest;
					code = HttpStatusCode.BadRequest;
					break;
				default:
					type = ErrorType.Unexpected;
					code = HttpStatusCode.InternalServerError;
					break;
			}
			// тут message надо доработать 
			// и локализацию если в будущем решим
			return (code, type, exception.Message);
		}
	}
}
