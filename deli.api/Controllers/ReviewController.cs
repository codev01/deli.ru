using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[Route("[controller].[action]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
		public ActionResult Get([Required] int reviewId)
		{
			return null;
		}
	}
}
