using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[Route("[controller].[action]")]
	[ApiController]
	public class LandlordController : ControllerBase
	{
		/// <summary>
		/// Получить информацию о пользователе, как о арендодателе
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>Информация о пользователе, как о арендодателе</returns>
		[Auth]
		[HttpGet]
		//public Landlord Get([Required] string userId)
		//{
		//	//Request.Form.Files 
		//	//return new Landlord() { Id = userId, RatingLandlord = 324 };
		//	return null;
		//}

		/// <summary>
		/// Получить список опубликованных учётных едениц
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="offset">Смещение с начала списка</param>
		/// <returns>список опубликованных учётных едениц</returns>
		[HttpGet]
		public IEnumerable<Product> GetPublishedProducts(int userId, int offset = 0)
		{
			return new List<Product>() { new Product(), new Product() };
		}

		/// <summary>
		/// Получить список неопубликованных учётных едениц
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="offset">Смещение с начала списка</param>
		/// <returns>Список неопубликованных учётных едениц</returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
		public ActionResult GetUnpublishedProducts([Required] int userId, int offset = 0)
		{
			return null;
		}

		/// <summary>
		/// Получить отзывы оставленные арендодателю по идентификатору пользователя
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="offset">Смещение с начала списка</param>
		/// <returns>Отзывы оставленные арендодателю</returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		public ActionResult GetReviews([Required] int userId, int offset = 0)
		{
			return null;
		}
	}
}
