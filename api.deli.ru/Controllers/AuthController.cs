using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using api.deli.ru.Constants;
using api.deli.ru.Services.Contracts;

using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

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

        [HttpPost]
		public async Task<IActionResult> Registration(/*...*/)
		{
			throw new NotImplementedException();
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login([Required] string app_id, 
											   [Required] string client_secret, 
											   [Required] string account_id, 
											   [Required] string password){
			try
			{
				//App app = (await _authService.GetById(app_id)).First();
				//Account account = (await _accountService.GetById(account_id)).First();
				App app = Apps.FirstOrDefault(x => x.Id.Value == app_id && x.ClientSecret == client_secret);
				Account account = People.FirstOrDefault(x => x.Id.Value == account_id &&
														x.Password == password);
				var claims = _authService.GetAppIdentity(app).Claims.Union(_authService.GetUserIdentity(account).Claims);
				var identity = new ClaimsIdentity(claims);

				string token = _authService.GenerateToken(identity);

				// тут надо проверить был ли изменён у пользователя пароль до авторизации по полю "isLogined"
				if(!account.IsLogined)
				{
					// изменяем флаг в аккаунте в БД на true
				}


				return Ok(new { user_name = account_id, token = token });
			}
			catch (Exception e)
			{
				return Unauthorized(new { errorText = e.Message, error_obj = JsonConvert.SerializeObject(e) });
			}
		}
		private static List<Account> People = new List<Account>
		{
			new Account("int", "00000") { Email = "admin@gmail.com", Role = "landlord" },
			new Account("rx1310", "11111") { Email = "analitik@gmail.com", Role = "tenant" },
			new Account("codev01", "22222") { Email = "developer@gmail.com", Role = "landlord" },
			new Account("tomodachi", "54321") { Email = "developer@gmail.com", Role = "lendlord" },
			new Account("gambit", "00001") { Email = "qwerty@gmail.com", Role = "tenant" }
		};
		private static List<App> Apps = new List<App>
		{
			new App("000065805674abf941506bb1", "qwertyuiop") { Scopes = new[]{ AppScopes.All }, Version = 1.0 },
			new App("000065805674abf941506bb2", "asdfghjkl") { Scopes = new[]{ AppScopes.Categories }, Version = 1.2}
		};

	}
}
