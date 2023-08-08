using System.ComponentModel.DataAnnotations;

using api.deli.ru.Constants;

using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;
using data.deli.ru.Parameters;

using Microsoft.AspNetCore.Mvc;

namespace api.deli.ru.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Policy = Policies.TenantAndLandlord, Scopes = Scopes.Announcements)]
	public class AnnouncementController : Controller
	{
		private readonly IAnnouncementService _announcementService;
		public AnnouncementController(IAnnouncementService categoryService)
			=> _announcementService = categoryService;

		[HttpGet]
		public async Task<IEnumerable<Announcement>> GetById([FromQuery] string[] categoryIds)
			=> await _announcementService.GetById(categoryIds);

		[HttpGet]
		public async Task<IEnumerable<Announcement>> GetAnnouncements([Required] string searchString,
																	  string categoryId,
																	  bool? isPublished,
																	  [FromQuery] Radius radius,
																	  [FromQuery] Constraint constraint)
			=> await _announcementService.GetAnnouncements(searchString, categoryId, isPublished, radius, constraint);

		//[HttpPost]
		//public async Task<IActionResult> AddAnnouncement(Announcement)
	}
}