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
	[Auth(Roles = Roles.All, Scopes = Scopes.Serach)]
	public class SearchController : Controller
	{
		private readonly IAnnouncementService _announcementService;
		private readonly IProductService _productService;
		public SearchController(IAnnouncementService announcementService, IProductService productService)
		{
			_announcementService = announcementService;
			_productService = productService;
		}

		[HttpGet]
		public async Task<IEnumerable<Announcement>> SearchAnnouncements([Required] string searchString,
																		 string categoryId,
																		 [FromQuery] Duration duration,
																		 [FromQuery] Radius radius,
																		 [FromQuery] PriceMaxMin price,
																		 [FromQuery] Constraint constraint)
		{
			var announcements = await _announcementService.GetAnnouncements(searchString, categoryId, true, radius, constraint);
			foreach (Announcement announcement in announcements)
			{
				var product = (await _productService.GetProducts(announcement.Id, price, duration, constraint)).
					OrderBy(p => p.RentPrice).
					ThenBy(p => p.FullPrice).
					FirstOrDefault();

				announcement.PreviewProduct = product;
				continue;
			}

			return announcements;
		}
	}
}
