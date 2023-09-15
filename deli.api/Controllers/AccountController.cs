using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	public class AccountController : Controller
	{
		[HttpPut]
		[Auth]
		public async Task<IActionResult> ChangeEmail()
		{
			throw new NotImplementedException();
		}

		[HttpPut]
		[Auth]
		public async Task<IActionResult> ChangePassword(/*ChangePasswordModel model*/)
		{
			// после изменения пароля выставить isLogined в false


			//// Проверка текущего пользователя
			//var user = await _userManager.GetUserAsync(User);
			//if (user == null)
			//{
			//	return Unauthorized();
			//}

			//// Изменение пароля
			//var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			//if (!result.Succeeded)
			//{
			//	return BadRequest(result.Errors);
			//}

			//// Аннулирование всех токенов пользователя
			//await _userManager.UpdateSecurityStampAsync(user);

			//// Отключение текущей аутентификации
			//await HttpContext.SignOutAsync();

			//return Ok();

			throw new NotImplementedException();
		}
	}
}
