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
	[Auth(Policy = Policies.TenantAndLandlord, Scopes = Scopes.Announcements)]
	public class AnnouncementController : Controller
	{
		private readonly IAnnouncementService _announcementService;
		public AnnouncementController(IAnnouncementService categoryService)
			=> _announcementService = categoryService;

		[HttpGet]
		public async Task<IEnumerable<Announcement>> GetById([FromQuery] string[] categoryIds)
			=> await _announcementService.GetById(categoryIds);

		[HttpPost]
		public async Task<IActionResult> AddAnnouncement([Required] Announcement announcement)
		{
			await _announcementService.AddAnnouncement(announcement);
			return Ok();
		}

		[HttpGet]
		public async Task<IEnumerable<Announcement>> GetAnnouncements([Required] string searchString,
																	  string categoryId,
																	  bool? isPublished,
																	  [FromQuery] Radius radius,
																	  [FromQuery] Constraint constraint)
			=> await _announcementService.GetAnnouncements(searchString, categoryId, isPublished, radius, constraint);
	}
}