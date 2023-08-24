using System.Net;

namespace deli.api.Extensions
{
	public static class HttpResponseExtensions
	{
		public static async Task WriteResponseAsync(this HttpResponse response, object obj, HttpStatusCode statusCode)
		{
			response.ContentType = "application/json";
			response.StatusCode = (int)statusCode;
			await response.WriteAsync(Utils.JsonSerializer(obj));
		}
	}
}
