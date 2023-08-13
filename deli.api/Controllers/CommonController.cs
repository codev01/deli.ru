﻿using deli.api.Constants;

using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Roles = Roles.All, Scopes = Scopes.All)]
	public class CommonController : Controller
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