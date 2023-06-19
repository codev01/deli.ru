using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using api.deli.ru.Services.Contracts;

using data.deli.ru.Common;
using data.deli.ru.MongoDB.Services.Contracts;
using data.deli.ru.Types;

using Microsoft.AspNetCore.Mvc;

namespace api.deli.ru.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	public class ProductController : Controller, IProductService
	{
		//private readonly IFileService _fileService;

		private IProductService _productService { get; }

		public ProductController(/*IFileService fileService*/ IProductService productService)
		{
			//_fileService = fileService;
			_productService = productService;
		}

		[HttpGet]
		public Task<IEnumerable<Product>> GetById(string[] ids) 
			=> _productService.GetById(ids);
		
		[HttpGet]
		public async Task<IEnumerable<Product>> GetProducts([Required] string searchString,
																	   string categoryId,
																	   double minRentPrice = DefaultParams.MIN_PRICE,
																	   double maxRentPrice = DefaultParams.MAX_PRICE,
																	   double minFullPrice = DefaultParams.MIN_PRICE,
																	   double maxFullPrice = DefaultParams.MAX_PRICE,
																	   double startLatitude = Location.MIN_LATITUBE,
																	   double startLongitude = Location.MIN_LONGITUBE,
																	   double endLatitude = Location.MAX_LATITUBE,
																	   double endLongitude = Location.MAX_LONGITUBE,
																	   uint limit = DefaultParams.LARGE_LIMIT,
																	   uint offset = DefaultParams.OFFSET_DEFAULT)
			=> await _productService.GetProducts(searchString, 
												 categoryId, 
												 minRentPrice, 
												 maxRentPrice, 
												 minFullPrice, 
												 maxFullPrice, 
												 startLatitude, 
												 startLongitude, 
												 endLatitude, 
												 endLongitude, 
												 limit, 
												 offset);
		

		/// <summary>
		/// Получить список отзывов по идентификатору учётной еденицы
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="offset">Смещение с начала списка</param>
		/// <returns>Список отзывов учётной еденицы</returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		public ActionResult GetReviews(int qwe, int qweew, [FromQuery]params int[] radius)
		{
			return null;
		}


		[Auth]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PostAttachments(string user_id)
		{
			IFormFileCollection requestFiles = Request.Form.Files;

			long size = requestFiles.Sum(f => f.Length);

			foreach (var formFile in Request.Form.Files)
			{
				if (formFile.Length > 0)
				{
					var filePath = Path.GetTempFileName();

					using (var stream = System.IO.File.Create(filePath))
					{
						await formFile.CopyToAsync(stream);
					}
				}
			}

			//_fileService.UploadFiles(requestFiles, user_id);


			if (requestFiles.Count < 1 && requestFiles.Count > 1)
				return new BadRequestResult();

			//_fileService.UploadFileAsync(requestFiles.First(), user_id);

			return Ok(new { count = requestFiles.Count, size });
		}
	}
}
