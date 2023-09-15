using System.Security.Claims;

namespace deli.api.Extensions
{
	public static class ClaimsExtension
	{
		public static Claim GetClaim(this IEnumerable<Claim> claims, string type)
			=> claims.First(claim => claim.Type == type);
		public static IEnumerable<Claim> GetClaims(this IEnumerable<Claim> claims, string type)
			=> claims.Where(claim => claim.Type == type);
	}
}
