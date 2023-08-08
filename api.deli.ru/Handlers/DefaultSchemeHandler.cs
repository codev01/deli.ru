using System.Text.Encodings.Web;

using api.deli.ru.Constants;
using api.deli.ru.Services.Contracts;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace api.deli.ru.Handlers
{
	/// <summary>
	/// Класс для хранения параметров схемы аутентификации
	/// </summary>
	public class DefaultSchemeOptions : AuthenticationSchemeOptions
	{
		//public string ApiKey { get; set; }
		//public bool RequireHttps { get; set; }
	}

	public class DefaultSchemeHandler : AuthenticationHandler<DefaultSchemeOptions>
	{
		private readonly IAuthService _authService;
		public DefaultSchemeHandler(IOptionsMonitor<DefaultSchemeOptions> options,
								 ILoggerFactory logger,
								 UrlEncoder encoder,
								 ISystemClock clock,
								 IAuthService authService) : base(options, logger, encoder, clock)
		{
			_authService = authService;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			// пример получения свойств
			//Options.ApiKey

			var context = Context.Request.HttpContext;
			Endpoint endpoint = context.GetEndpoint();
			string token = string.Empty;

			try
			{
#if DEBUG_AUTHDISABLE
				context.Request.Headers[Headers.TokenHeaderName] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6ImNvZGV2MDEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJsYW5kbG9yZCIsImFwcElkIjoiNjQ5MjA0OGZkMTAzOGJmNThjNGEzMGQ2IiwiYXBwVmVyc2lvbiI6IjEiLCJzY29wZSI6WyJjYXRlZ29yaWVzIiwiY2l0aWVzIiwic2VhcmNoIiwidXNlcnMiLCJmZWVkYmFja3MiLCJwcm9kdWN0cyIsImFubm91bmNlbWVudHMiLCJhdXRoIl0sIm5iZiI6MTY4OTM5NTI3MiwiZXhwIjoxNjkxOTg3MjcyLCJpc3MiOiJhdXRoLmRlbGkucnUiLCJhdWQiOlsiYXBpLmRlbGkucnUiLCJhcGkuZGVsaS5ydSJdfQ.XPerBKqehsmlwJJD_9wT9YaZ0vO8fasKqgv4i525IPk";
#endif
				// проверяем заголовок
				if (context.Request.Headers.TryGetValue(Headers.TokenHeaderName, out var headerValue))
				{
					if (string.IsNullOrEmpty(headerValue))
						throw new Exception($"\"{Headers.TokenHeaderName}\": null or empty");
					else
						token = headerValue;

					context.Request.Headers[Headers.TokenHeaderName] = $"{Scheme.Name} {token}";

					// проверяем токен
					var claimsPrincipal = await _authService.ValidateToken(token, _authService.GetTokenValidationParameters());

					var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
					return AuthenticateResult.Success(ticket);
				}
				else
					throw new Exception($"Not header \"{Headers.TokenHeaderName}\"");
			}
			catch (Exception e)
			{
				return AuthenticateResult.Fail(e.Message);
			}
		}
	}
}
