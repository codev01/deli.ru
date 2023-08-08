using api.deli.ru.Constants;

using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace api.deli.ru.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Roles = Roles.All, Scopes = Scopes.Categories)]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
			=> _categoryService = categoryService;

		[HttpGet]
		public async Task<IEnumerable<Category>> GetById([FromQuery] string[] categoryIds)
		{
			return await _categoryService.GetById(categoryIds);
		}

		[HttpGet]
		public async Task<IEnumerable<Category>> GetCategories()
		{
			return await _categoryService.GetCategories();
		}
	}
}
