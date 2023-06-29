using api.deli.ru.Constants;
using data.deli.ru.MongoDB.Models;
using data.deli.ru.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace api.deli.ru.Controllers
{
    [ApiController]
	[Route("[controller].[action]")]
	[Auth(Roles = Roles.All, Scopes = AppScopes.Serach)]
	public class SearchController
	{
        public SearchController()
        {
            
        }

  //      [HttpGet]
		//public async Task<IEnumerable<Announcement>> GetAnnouncements(string searchString,
		//															  string categoryId,
		//															  Radius radius,
		//															  Constraint constraint)
		//{


		//	await _announcementService.GetAnnouncements(searchString, categoryId, startLatitude, startLongitude, endLatitude, endLongitude, limit, offset);
		//}

	}
}
