using Microsoft.AspNetCore.Mvc;

namespace deli.api.Controllers
{
	[Auth]
	[ApiController]
	[Route("[controller].[action]")]
	public class TransactionController
	{
		/// <summary>
		/// Забронировать
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult UpdateReservation(int productId)
		{
			return null; //"updated successfully";
		}

		[HttpPut]
		public ActionResult UpdateTransaction(int productId)
		{
			return null; //"updated successfully";
		}
	}
}
