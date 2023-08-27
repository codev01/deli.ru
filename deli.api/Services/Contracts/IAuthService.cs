using System.Security.Claims;

using deli.data.MongoDB.Models;

using Microsoft.IdentityModel.Tokens;

namespace deli.api.Services.Contracts
{
	public interface IAuthService
	{
		string GenerateToken(ClaimsIdentity identity);
		ClaimsIdentity GetAppIdentity(App app);
		ClaimsIdentity GetUserIdentity(Account account);
		Claim CreateClaim(string type, string value);
		Task<ClaimsPrincipal> ValidateToken(string token, TokenValidationParameters validationParameters);
		TokenValidationParameters GetTokenValidationParameters();
	}
}
