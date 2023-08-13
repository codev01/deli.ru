using deli.api.Services.Contracts;


using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	public class UserController : ControllerBase
	{
		private readonly IFileService _fileService;
		public UserController(IFileService fileService)
		{
			_fileService = fileService;
		}
		/// <summary>
		/// Получить текущего пользователя.
		/// В случае если надо получить доп. инфу
		/// </summary>
		/// <returns></returns>
		[Auth]
		[HttpGet]
		[ProducesResponseType(typeof(UserPrivateInfo), StatusCodes.Status200OK)]
		public UserPrivateInfo GetConfidentionalInfo()
		{
			return null;
		}

		/// <summary>
		/// Получить список избранных учётных едениц.
		/// Так как и Tenant и Landlord могут иметь избранные учётные единицы
		/// </summary>
		/// <returns>Список избранных учётных едениц</returns>
		[Auth]
		[HttpGet]
		[ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
		public ActionResult GetFavorites()
		{
			return null;
		}

		/// <summary>
		/// Получить краткую информацию о пользователе
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>Краткая информация о пользователе</returns>
		[HttpGet]
		[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
		public ActionResult GetUserInfo(int userId)
		{
			return null;
		}


		[Auth]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult PostAvatar(IFormFile file, string user_id)
		{
			IFormFileCollection requestFiles = Request.Form.Files;

			if (requestFiles.Count < 1 && requestFiles.Count > 1)
				return new BadRequestResult();

			_fileService.UploadFileAsync(requestFiles.First(), user_id);

			return new OkResult();
		}
	}
}
