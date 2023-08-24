using System.Text.Encodings.Web;

using deli.api.Constants;
using deli.api.Services.Contracts;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace deli.api.Handlers
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
			
			try
			{
#if DEBUG_AUTHDISABLE
				context.Request.Headers[Headers.TokenHeaderName] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6ImNvZGV2MDEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJsYW5kbG9yZCIsImFwcElkIjoiNjQ5MjA0OGZkMTAzOGJmNThjNGEzMGQ2IiwiYXBwVmVyc2lvbiI6IjEiLCJzY29wZSI6WyJjYXRlZ29yaWVzIiwiY2l0aWVzIiwic2VhcmNoIiwidXNlcnMiLCJmZWVkYmFja3MiLCJwcm9kdWN0cyIsImFubm91bmNlbWVudHMiLCJhdXRoIl0sIm5iZiI6MTY5MTg5NTc0NCwiZXhwIjoxNjk0NDg3NzQ0LCJpc3MiOiJkZWxpLmFwaSIsImF1ZCI6WyJkZWxpLmNsaWVudCIsImRlbGkuY2xpZW50Il19.61HF9iUzuqpkqxvlDCLQrbYX7kgLfq_VdgqLSOJklZ8";
#endif
				Context.Request.SetScheme(Scheme.Name);
				
				// проверяем токен
				var claimsPrincipal = await _authService.ValidateToken(Request.GetToken(), _authService.GetTokenValidationParameters());

				var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
				return AuthenticateResult.Success(ticket);
			}
			catch (Exception e)
			{
				return AuthenticateResult.Fail(e.Message);
			}
		}
	}
}
