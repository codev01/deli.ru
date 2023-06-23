using api.deli.ru.Constants;
using api.deli.ru.Extensions;

using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace api.deli.ru.Middlewares
{
	public class AuthMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IAppService _appService;
		private readonly IAccountService _accountService;

		public AuthMiddleware(RequestDelegate next, IAppService appService, IAccountService accountService)
		{
			_next = next;
			_appService = appService;
			_accountService = accountService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				Endpoint endpoint = context.GetEndpoint();
				string token = string.Empty;

				if (endpoint is not null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() is null)
				{
					if (context.User.Identity.IsAuthenticated)
					{
						var appIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == JWTClaimTypes.AppId);
						var accountIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == JWTClaimTypes.UserName);
						var app = (await _appService.GetById(appIdClaim.Value)).FirstOrDefault();
						var account = (await _accountService.GetAccount(accountIdClaim.Value));

						if (app is null)
							throw new Exception("Application does not exist");
						if (account is null)
							throw new Exception("User does not exist");

						// проверяем актуальный ли токен
						// был ли изменён пароль или такого пользователя больше не существует
						if (!account.IsLogined)
						{
							context.Response.StatusCode = StatusCodes.Status401Unauthorized;
							await context.Response.WriteResponseAsync($"Changed password or delited account", StatusCodes.Status401Unauthorized);
							return;
						}

						var authAttribute = endpoint.Metadata.GetMetadata<AuthAttribute>();

						if (authAttribute is not null && !string.IsNullOrEmpty(authAttribute.Scopes))
						{
							string[] attributeScopes = authAttribute.Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

							bool isMatched = false;
							foreach (string scope in attributeScopes)
							{
								if (Array.Exists(app.Scopes, s => s == scope))
									isMatched = true;
								else
									isMatched = false;
							}
							if (!isMatched)
							{
								await context.Response.WriteResponseAsync($"App does not have access. Required access: {authAttribute.Scopes}", StatusCodes.Status403Forbidden);
								return;
							}
						}
					}
					else
					{
						AuthenticateResult authenticateResult = await context.AuthenticateAsync();
						if (authenticateResult.Failure is not null)
							throw authenticateResult.Failure;
						else
							throw new Exception("Authentication failed for unknown reason");
					}
				}

				await _next(context);
			}
			catch (Exception e)
			{
				await context.Response.WriteResponseAsync("Invalid token. Error message: " + e.Message, StatusCodes.Status401Unauthorized);
			}
		}
	}
}
