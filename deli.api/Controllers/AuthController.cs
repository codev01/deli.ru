using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using deli.api.Constants;
using deli.api.Extensions;
using deli.api.Services.Contracts;

using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;
using deli.runtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using NuGet.Common;

namespace deli.api.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Scopes = Scopes.Auth)]
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IAppService _appService;
		private readonly IAccountService _accountService;

		public AuthController(IAuthService authService, IAppService appService, IAccountService accountService)
		{
			_authService = authService;
			_appService = appService;
			_accountService = accountService;
		}

		[HttpGet]
		public async Task<IActionResult> Registration(/*...*/)
		{
			throw new NotImplementedException();
		}

		[HttpGet]
		[Auth(Roles = Roles.Guest)]
		public async Task<IActionResult> UserLogin([Required] string user_name,
												   [Required] string password)
		{
			try
			{
				Account account = (await _accountService.GetAccount(user_name));
				if (account is null)
					return BadRequest("User does not exist");
				if (account.Password != password)
					return Unauthorized("Incorrect password");

				var identity = _authService.GetUserIdentity(account);
				identity.AddClaims(User.Claims);
				identity.RemoveClaim(identity.Claims.GetClaim(ClaimTypes.Role));
				string token = _authService.GenerateToken(identity);

				// был ли изменён у пользователя пароль до авторизации по полю "isLogined"
				if (!account.IsLogined)
					// изменяем флаг в аккаунте в БД на true
					_accountService.UpdateField(account.Id, a => a.IsLogined, true);

				_ContainerManager.AddUser(new runtime.Containers.UserContainer_(user_name, token, HttpContext.Connection.RemoteIpAddress));

				return Ok(new { user_name = user_name, token = token });
			}
			catch (Exception e)
			{
				return Unauthorized(new { errorText = e.Message });
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> AppLogin([Required] string appId,
												  [Required] string clientSecret)
		{
			try
			{
				App app = (await _appService.GetById(appId)).FirstOrDefault();
				if (app is null)
					return BadRequest("Application does not exist");
				if (app.ClientSecret != clientSecret)
					return Unauthorized("Incorrect secret");

				var identity = _authService.GetAppIdentity(app);
				string token = _authService.GenerateToken(identity);
				_ContainerManager.AddApp(new runtime.Containers.ApplicationContainer_(appId, token, app.RateLimit));

				return Ok(new { app_name = app.Name, token = token });
			}
			catch (Exception e)
			{
				return Unauthorized(new { errorText = e.Message });
			}
		}

		[HttpGet]
		[Auth(Roles = Roles.Tenant)]
		public async Task<IActionResult> ChangeRoleToLandlord()
		{
			var accountIdClaim = User.Claims.GetClaim(JWTClaimTypes.UserName).Value;
			Account account = (await _accountService.GetAccount(accountIdClaim));

			if (account.Roles.Any(r => r == Roles.Landlord))
				return BadRequest($"User already has a role '{Roles.Landlord}'");

			var identity = new ClaimsIdentity(User.Claims);
			identity.AddClaim(new Claim(JWTClaimTypes.Roles, Roles.Landlord));
			var roles = identity.Claims.GetClaims(ClaimTypes.Role).ToArray();
			foreach (Claim claim in roles)
			{
				identity.RemoveClaim(claim);
				identity.AddClaim(_authService.CreateClaim(JWTClaimTypes.Roles, claim.Value));
			}
			_accountService.AddItemArray(account.Id, a => a.Roles, Roles.Landlord);
			string token = _authService.GenerateToken(identity);

			return Ok(new { user_name = account.UserName, token = token});
		}
	}
}
