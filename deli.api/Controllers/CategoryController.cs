using deli.api.Constants;

using deli.data.MongoDB.Services.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
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
			=> await _categoryService.GetById(categoryIds);

		[HttpGet]
		public async Task<IEnumerable<Category>> GetCategories()
			=> await _categoryService.GetCategories();
	}
}
