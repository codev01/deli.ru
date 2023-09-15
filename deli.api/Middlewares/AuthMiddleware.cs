using System.Net;
using System.Security.Claims;

using deli.api.Constants;
using deli.api.Exceptions;
using deli.api.Extensions;
using deli.api.Handlers;
using deli.data.MongoDB.Services.Contracts;
using deli.runtime;
using deli.runtime.Containers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace deli.api.Middlewares
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
				if (endpoint == null)
					throw new NullReferenceException("endpoint = null");

				#region Attributes
				var authAttribute = endpoint.Metadata.GetMetadata<AuthAttribute>();
				var allowAnonymousAttribute = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
				#endregion

				// если применён атрибут AllowAnonymous то просто проходим мимо
				if (allowAnonymousAttribute is null && authAttribute is not null)
				{
					// Прошли ли аутентификацию
					if (context.User.Identity.IsAuthenticated)
					{
						string token = context.Request.GetToken();

						var claims = context.User.Claims;

						foreach (Claim claim in claims.GetClaims(JWTClaimTypes.Identities))
						{
							switch (claim.Value)
							{
								case nameof(TypeIdentity.Application):
									var appIdClaim = claims.GetClaim(JWTClaimTypes.AppId);
									var applicationContainer = _ContainerManager.GetApp(appIdClaim?.Value);

									if (applicationContainer is not null &&
										!applicationContainer.IsTerminated &&
										applicationContainer.ValidateToken(token))
									{
										if (applicationContainer.CheckRateLimit())
											applicationContainer.Update();
										else
											throw new ResponseException($"The number of method calls per second has been reached. This application has a 'RateLimit' = {applicationContainer.RateLimit}", HttpStatusCode.TooManyRequests);
									}
									else
										await CheckAppAuthDatabase(context, authAttribute, appIdClaim);

									break;

								case nameof(TypeIdentity.User):
									var userNameClaim = claims.GetClaim(JWTClaimTypes.UserName);
									var userContainer = _ContainerManager.GetUser(userNameClaim?.Value);

									if (userContainer is not null &&
										!userContainer.IsTerminated &&
										userContainer.ValidateToken(token))
									{
										if (userContainer.CheckIPAddress(context.Connection.RemoteIpAddress))
											userContainer.Update();
										else
											throw new ResponseException($"The number of address changes has been reached. This user can change address every {UserContainer_.REMOTE_IP_UPDATE_MILLISECONDS} seconds", HttpStatusCode.TooManyRequests);
									}
									else
										await CheckUserAuthDatabase(context, authAttribute, userNameClaim);

									break;
							}
						}
					}
					else
					{
						AuthenticateResult authenticateResult = await context.AuthenticateAsync();
						if (authenticateResult.Failure is not null)
							throw new ResponseException(authenticateResult.Failure, HttpStatusCode.Unauthorized);
						else
							throw new ResponseException("Authentication failed for unknown reason", HttpStatusCode.Unauthorized);
					}
				}

				await _next(context);
			}
			catch (ResponseException e)
			{
				await context.Response.WriteResponseAsync(e.Message, e.StatusCode);
				return;
			}
			catch (Exception e)
			{
				await context.Response.WriteResponseAsync("Unexpected error. Error message: " + e.Message, HttpStatusCode.InternalServerError);
			}
		}

		private async Task CheckAppAuthDatabase(HttpContext context, AuthAttribute authAttribute, Claim appIdClaim)
		{
			var app = (await _appService.GetById(appIdClaim.Value)).FirstOrDefault();

			if (app is null)
				throw new ResponseException("Application does not exist", HttpStatusCode.Unauthorized);

			if (!string.IsNullOrEmpty(authAttribute.Scopes))
			{
				string[] attributeScopes = authAttribute.Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

				bool isMatch = false;
				// если есть хотя бы один доступ
				foreach (string scope in attributeScopes)
				{
					if (app.Scopes.Any(s => s == scope))
					{
						isMatch = true;
						break;
					}
					else
						isMatch = false;
				}
				if (!isMatch)
					throw new ResponseException($"App does not have access. Required access: {authAttribute.Scopes}", HttpStatusCode.Forbidden);
			}
			_ContainerManager.AddApp(new ApplicationContainer_(appIdClaim.Value, context.Request.GetToken(), app.RateLimit));
		}

		private async Task CheckUserAuthDatabase(HttpContext context, AuthAttribute authAttribute, Claim userNameClaim)
		{
			var account = (await _accountService.GetAccount(userNameClaim.Value));

			if (account is null)
				throw new ResponseException("User does not exist", HttpStatusCode.Unauthorized);

			// был ли изменён пароль
			if (!account.IsLogined)
				throw new ResponseException($"Changed password or deleted account", HttpStatusCode.Unauthorized);

			if (!string.IsNullOrEmpty(authAttribute.Roles))
			{
				string[] attributeRoles = authAttribute.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

				bool isMatch = false;
				// если есть хотя бы одна роль
				foreach (string role in attributeRoles)
				{
					if (account.Roles.Any(r => r == role))
					{
						isMatch = true;
						break;
					}
					else
						isMatch = false;
				}
				if (!isMatch)
					throw new ResponseException($"User does not have one or more roles. Required roles: {authAttribute.Roles}", HttpStatusCode.Forbidden);

				_ContainerManager.AddUser(new UserContainer_(userNameClaim.Value, context.Request.GetToken(), context.Connection.RemoteIpAddress));
			}
		}
	}
}
