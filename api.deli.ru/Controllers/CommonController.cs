using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace api.deli.ru.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	public class CommonController : Controller, ICityService
	{
		private ICityService _cityService;

		public CommonController(ICityService cityService)
		{
			_cityService = cityService;
		}

		[HttpGet]
		public Task<IEnumerable<City>> SearchCities(string searchString, int limit = 100, int offset = 0)
			=> _cityService.SearchCities(searchString, limit, offset);
	}
}
