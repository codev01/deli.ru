using api.deli.ru.Constants;

using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;
using data.deli.ru.Parameters;

using Microsoft.AspNetCore.Mvc;

namespace api.deli.ru.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Roles = Roles.All, Scopes = AppScopes.Categories)]
	public class AnnouncementController : Controller
	{
		private readonly IAnnouncementService _announcementService;
		public AnnouncementController(IAnnouncementService categoryService)
			=> _announcementService = categoryService;

		[HttpGet]
		public async Task<IEnumerable<Announcement>> GetById([FromQuery] string[] categoryIds)
			=> await _announcementService.GetById(categoryIds);

		[HttpGet]
		public async Task<IEnumerable<Announcement>> GetPublishedAnnouncements(string searchString,
																			   string categoryId,
																			   [FromQuery] Radius radius,
																			   [FromQuery] Constraint constraint)
			=> await _announcementService.GetPublishedAnnouncements(searchString, categoryId, radius, constraint);
	}
}