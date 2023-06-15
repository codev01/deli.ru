using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using data.deli.ru.Models;
using System.Drawing;
using data.deli.ru.Types;
using data.deli.ru.MongoDB.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace api.deli.ru.Controllers
{
    [ApiController]
	[Route("[controller].[action]")]
	public class CategoryController : ControllerBase, ICategoryService
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService) 
			=> _categoryService = categoryService;

		[HttpGet]
		public async Task<IEnumerable<Category>> GetById([FromQuery]string[] categoryIds)
		{
			return await _categoryService.GetById(categoryIds);
		}

		[Auth(Policy = "landlord")]
		[HttpGet]
		public async Task<IEnumerable<Category>> GetCategories()
		{
			return await _categoryService.GetCategories();
		}
	}
}
