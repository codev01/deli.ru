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
				// проверяем заголовок
				if (context.Request.Headers.TryGetValue(Headers.TokenHeaderName, out var headerValue))
				{
					if (string.IsNullOrEmpty(headerValue))
						throw new Exception($"Not header \"{Headers.TokenHeaderName}\"");

					token = headerValue;
					context.Request.Headers[Headers.TokenHeaderName] = "Bearer " + token;

					// проверяем токен
					var claimsPrincipal = await _authService.ValidateToken(token, _authService.GetTokenValidationParameters());

					var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
					return AuthenticateResult.Success(ticket);
				}
				else
				{
					throw new Exception($"Not header \"{Headers.TokenHeaderName}\"");
				}
			}
			catch (Exception e)
			{
				return AuthenticateResult.Fail(e.Message);
			}
		}
	}
}
