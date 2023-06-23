namespace api.deli.ru.Extensions
{
	public static class HttpResponseExtensions
	{
		public static async Task WriteResponseAsync(this HttpResponse response, object obj, int statusCode)
		{
			response.ContentType = "application/json";
			response.StatusCode = statusCode;
			await response.WriteAsync(Utils.JsonSerializer(obj));
		}
	}
}
