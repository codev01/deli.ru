using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using api.deli.ru.Constants;
using api.deli.ru.Services.Contracts;

using data.deli.ru.MongoDB.Models;

using Microsoft.IdentityModel.Tokens;

namespace api.deli.ru.Services
{
	public class AuthService : IAuthService
	{
		public class AuthOptions
		{
			public const string ISSUER = "auth.deli.ru"; // издатель токена
			public const string AUDIENCE = "api.deli.ru"; // потребитель токена
			const string KEY = @"1234567890йцукенгшщз";   // ключ для шифрации
			public const int LIFETIME = 43200; // время жизни токена - 1 минута
						
			public static SymmetricSecurityKey GetSymmetricSecurityKey()
			{
				return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
			}
		}

		public string GenerateToken(ClaimsIdentity identity)
		{
			var now = DateTime.UtcNow;
			// создаем JWT-токен
			var jwt = new JwtSecurityToken
			(
					issuer: AuthOptions.ISSUER,
					audience: AuthOptions.AUDIENCE,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
																 SecurityAlgorithms.HmacSha256)
			);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		public Task<ClaimsPrincipal> ValidateToken(string token, TokenValidationParameters validationParameters)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				return Task.Run(() => tokenHandler.ValidateToken(token, validationParameters, out _));
			}
			catch
			{
				throw new Exception("Not valid token");
			}
		}

		public ClaimsIdentity GetAppIdentity(App app)
		{
			if (app != null)
			{
				var claims = new List<Claim>
				{
					new Claim(JWTClaimTypes.AppId, app.Id.StringId),
					new Claim(ClaimTypes.Role, Roles.Guest),
					new Claim(JWTClaimTypes.AppVersion, app.Version.ToString())
				};
				foreach (var scope in app.Scopes)
					claims.Add(new Claim(JWTClaimTypes.Scope, scope));

				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
				return claimsIdentity;
			}
			else
				// если приложения не найдено
				throw new ArgumentException("Invalid app_id or client_secret.");
		}

		public ClaimsIdentity GetUserIdentity(Account account)
		{
			if (account != null)
			{
				var claims = new List<Claim>
				{
					new Claim(JWTClaimTypes.UserName, account.UserName),
					new Claim(ClaimTypes.Role, account.Role)
				};
				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
				return claimsIdentity;
			}
			else
				// если пользователя не найдено
				throw new ArgumentException("Invalid user_name or password.");
		}

		public TokenValidationParameters GetTokenValidationParameters() => new TokenValidationParameters
			{
				ValidateIssuer = true, // Валидация источника токена
				ValidateAudience = true, // Валидация получателя токена
				ValidateLifetime = true, // Валидация срока действия токена
				ValidateIssuerSigningKey = true, // Валидация ключа подписи токена

				// источник токена
				ValidIssuer = AuthOptions.ISSUER,
				// получатель токена
				ValidAudience = AuthOptions.AUDIENCE,

				IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
			};
	}
}
