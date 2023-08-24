using deli.api.Constants;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using NuGet.Common;

namespace deli.api.Handlers
{
	public static class RequestExtension
	{
		public static string GetToken(this HttpRequest request)
		{
			if (request.Headers.TryGetValue(Headers.TokenHeaderName, out var headerValue))
			{
				if (!string.IsNullOrEmpty(headerValue))
					return headerValue;
				else
					throw new ArgumentNullException($"Header value '{Headers.TokenHeaderName}': null or empty");
			}
			else
				throw new Exception($"Not header '{Headers.TokenHeaderName}'");
		}

		public static void SetScheme(this HttpRequest request, string scheme)
		{
			string token = GetToken(request);

			request.Headers[Headers.TokenHeaderName] = token; //{scheme} 
		}
	}
}
