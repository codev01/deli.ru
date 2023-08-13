using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[Route("[controller].[action]")]
	[ApiController]
	public class TenantController : ControllerBase
	{
		/// <summary>
		/// Получить информацию о пользователе, как о арендаторе
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>Информация о пользователе, как о арендаторе</returns>
		[Auth]
		[HttpGet]
		//[ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
		//public ActionResult Get(int userId)
		//{
		//	return null;
		//}

		/// <summary>
		/// Получить отзывы оставленные арендатору по идентификатору пользователя
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="offset">Смещение с начала списка</param>
		/// <returns>Отзывы оставленные арендатору</returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		public ActionResult GetReviews(int userId, int offset = 0)
		{
			return null;
		}
	}
}
