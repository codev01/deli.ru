using deli.api.Constants;
using deli.data.MongoDB.Types;

using Microsoft.AspNetCore.Mvc;

namespace deli.api.Extensions
{
	public static class ControllerExtensions
	{
		public static BsonObjectId GetUserId(this Controller controller)
		{
			var userClaim = controller.User.Claims.GetClaim(JWTClaimTypes.UserId);
			return BsonObjectId.Create(userClaim.Value);
		}
	}
}
