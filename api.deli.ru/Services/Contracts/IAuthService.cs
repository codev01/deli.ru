using System.Security.Claims;

using data.deli.ru.MongoDB.Models;

using Microsoft.IdentityModel.Tokens;

namespace api.deli.ru.Services.Contracts
{
	public interface IAuthService
	{
		string GenerateToken(ClaimsIdentity identity);
		ClaimsIdentity GetAppIdentity(App app);
		ClaimsIdentity GetUserIdentity(Account account);
		ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters);
		TokenValidationParameters GetTokenValidationParameters();
	}
}
