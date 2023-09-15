using System.ComponentModel.DataAnnotations;

using deli.api.Constants;

using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;
using deli.data.Parameters;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Scopes = Scopes.Announcements)]
	public class AnnouncementController : Controller
	{
		private readonly IAnnouncementService _announcementService;
		public AnnouncementController(IAnnouncementService categoryService)
			=> _announcementService = categoryService;

		/// <summary>
		/// Получить 
		/// </summary>
		/// <remarks>
		/// Пример запроса:
		/// <code>
		///     POST /Todo
		///     {
		///        "id" : 1, 
		///        "name" : "A4Tech Bloody B188",
		///        "price" : 111,
		///        "Type": "PeripheryAndAccessories"
		///     }
		/// </code>
		///
		/// </remarks>
		/// <param name="announcementIds"> Идентификаторы категорий </param>
		/// <response code="200">Успешное выполнение</response>
		/// <response code="400">Ошибка API</response>
		/// <returns>вапукпвукппув</returns>
		[HttpGet]
		[Auth(Roles = Roles.All)]
		public async Task<IEnumerable<Announcement>> GetById([FromQuery] string[] announcementIds)
			=> await _announcementService.GetById(announcementIds);

		[HttpPost]
		[Auth(Roles = Roles.Landlord)]
		public async Task<IActionResult> AddAnnouncement([Required] Announcement announcement)
		{
			await _announcementService.AddAnnouncement(announcement);
			return Ok();
		}

		[HttpGet]
		[Auth(Roles = Roles.All)]
		public async Task<IEnumerable<Announcement>> GetAnnouncements([Required] string searchString,
																	  string categoryId,
																	  bool? isPublished,
																	  [FromQuery] Radius radius,
																	  [FromQuery] Constraint constraint)
			=> await _announcementService.GetAnnouncements(searchString, categoryId, isPublished, radius, constraint);
	}
}