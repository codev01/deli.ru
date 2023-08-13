using System.ComponentModel.DataAnnotations;

using deli.api.Constants;
using deli.data.MongoDB.Services.Contracts;
using deli.data.Parameters;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[ApiController]
	[Route("[controller].[action]")]
	[Auth(Roles = Roles.All, Scopes = Scopes.Products)]
	public class ProductController : Controller
	{
		//private readonly IFileService _fileService;

		private readonly IProductService _productService;

		public ProductController(/*IFileService fileService*/ IProductService productService)
		{
			//_fileService = fileService;
			_productService = productService;
		}

		[HttpGet]
		public Task<IEnumerable<Product>> GetById(string[] ids)
			=> _productService.GetById(ids);

		[HttpPost]
		public async Task<IActionResult> AddProducts(string announcementId, Product[] products)
		{
			await _productService.AddProduct(announcementId, products);
			return Ok();
		}

		[HttpGet]
		public async Task<IEnumerable<Product>> GetProducts([Required] string announcementId,
															[FromQuery] PriceMaxMin price,
															[FromQuery] Duration duration,
															[FromQuery] Constraint constraint) =>
			await _productService.GetProducts(announcementId, price, duration, constraint);


		/// <summary>
		/// Получить список отзывов по идентификатору учётной еденицы
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="offset">Смещение с начала списка</param>
		/// <returns>Список отзывов учётной еденицы</returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<Feedback>), StatusCodes.Status200OK)]
		public ActionResult GetReviews(int qwe, int qweew, [FromQuery] params int[] radius)
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
