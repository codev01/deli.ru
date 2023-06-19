using api.deli.ru.Constants;
using api.deli.ru.Services.Contracts;

using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace api.deli.ru.Middlewares
{
	public class AuthMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IAuthService _authService;
		private readonly IAppService _appService;
		private readonly IAccountService _accountService;

		public AuthMiddleware(RequestDelegate next, IAuthService authService, IAppService appService, IAccountService accountService)
		{
			_next = next;
			_authService = authService;
			_appService = appService;
			_accountService = accountService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				Endpoint endpoint = context.GetEndpoint();
				string token = string.Empty;

				if (!endpoint.Metadata.Any(a => a.GetType() == typeof(AllowAnonymousAttribute)))
				{
					// чекаем заголовок
					if (context.Request.Headers.TryGetValue(Headers.TokenHeaderName, out var headerValue))
					{
						token = headerValue;
						context.Request.Headers[Headers.TokenHeaderName] = "Bearer " + token;

						// проверяем токен
						var claimsPrincipal = _authService.ValidateToken(token, _authService.GetTokenValidationParameters());
						var appIdClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == JWTClaimTypes.AppId);
						var accountIdClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == JWTClaimTypes.AccountId);
						var app = (await _appService.GetById(appIdClaim.Value)).First();
						var account = (await _accountService.GetById(accountIdClaim.Value)).First();

						// проверяем актуальный ли токен
						// был ли изменён пароль или такого пользователя больше не существует
						if (!account.IsLogined)
						{
							context.Response.StatusCode = StatusCodes.Status401Unauthorized;
							await context.Response.WriteAsync($"Changed password or delited account");
							return;
						}

						// проверяем атрибуты конечных точек
						if (endpoint is not null)
						{
							//bool exitLoop = false;
							//foreach (var metadata in endpoint.Metadata)
							//{
							//	switch (metadata)
							//	{
							//		case AllowAnonymousAttribute:
							//			exitLoop = true;
							//			break;
							//		case AuthAttribute:
							//			break;
							//	}
							//	if (exitLoop) break;
							//}

							var authAttribute = endpoint.Metadata.GetMetadata<AuthAttribute>();

							if (authAttribute != null && !string.IsNullOrEmpty(authAttribute.Scopes))
							{
								string[] attributeScopes = authAttribute.Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

								bool isMatched = false;
								foreach (string scope in attributeScopes)
								{
									if (Array.Exists(app.Scopes, s => s == scope))
									{
										isMatched = true;
										break;
									}
								}
								if (!isMatched)
								{
									context.Response.StatusCode = StatusCodes.Status403Forbidden;
									await context.Response.WriteAsync($"App does not have access. Required access: {authAttribute.Scopes}");
									return;
								}
							}
						}
					}
					else
					{
						context.Response.StatusCode = StatusCodes.Status403Forbidden;
						await context.Response.WriteAsync($"Not header: {Headers.TokenHeaderName}");
						return;
					}
				}

				await _next(context);
			}
			catch (Exception e)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Invalid token. Error message: " + e.Message);
			}
			
		}
	}
}
