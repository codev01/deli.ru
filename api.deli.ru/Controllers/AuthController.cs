using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using api.deli.ru.Constants;
using api.deli.ru.Services.Contracts;

using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace api.deli.ru.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
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

				var claims = _authService.GetUserIdentity(account).Claims;
				var identity = new ClaimsIdentity(claims);

				string token = _authService.GenerateToken(identity);

				// был ли изменён у пользователя пароль до авторизации по полю "isLogined"
				if (!account.IsLogined)
				{
					// изменяем флаг в аккаунте в БД на true
					_accountService.UdateField(account.Id, a => a.IsLogined, true);
				}

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
												  [Required] string clientSecret){
			try
			{
				App app = (await _appService.GetById(appId)).FirstOrDefault();
				if (app is null)
					return BadRequest("Application does not exist");
				if (app.ClientSecret != clientSecret)
					return Unauthorized("Incorrect secret");

				var claims = _authService.GetAppIdentity(app).Claims;
				var identity = new ClaimsIdentity(claims);
				string token = _authService.GenerateToken(identity);

				return Ok(new { appToken = token });
			}
			catch (Exception e)
			{
				return Unauthorized(new { errorText = e.Message });
			}
		}
	}
}
