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

			public static string RSAPublicKey = "MIIBCgKCAQEAvqi2OswOHg4Th7UfDCfZNsO4BaiolopyKUp8yZ4ouUTOwYwYPxOqYzLX2RPvdEP9KFbFJLw8vXF0h8UGpokOQYjsVSV4xBVg8rIPM8fdAwwlm6s8Itg0qSiahcyHkJ5xGCt1P+3e4dmtDinA13FllUofptqcWGZ0dUCnQXYbRgtmxfWrGP73akwDTZFGrGVay5OTWO3ZdMoUCAKzOFaeCoA2FGpnq4XyLpm9z3WtGHtCcNhPndcWySDPoTf9U8wlCZiOLgZvFrOU4AEqshBL1g8DO0RrPG1nKyKDKVi7Zf1QijzVCEKnKiNw33Cs0OBhqHBJbvX/g9M1syVwCEASYQIDAQAB";
			public static string RSAPrivateKey = "MIIEpAIBAAKCAQEAvqi2OswOHg4Th7UfDCfZNsO4BaiolopyKUp8yZ4ouUTOwYwYPxOqYzLX2RPvdEP9KFbFJLw8vXF0h8UGpokOQYjsVSV4xBVg8rIPM8fdAwwlm6s8Itg0qSiahcyHkJ5xGCt1P+3e4dmtDinA13FllUofptqcWGZ0dUCnQXYbRgtmxfWrGP73akwDTZFGrGVay5OTWO3ZdMoUCAKzOFaeCoA2FGpnq4XyLpm9z3WtGHtCcNhPndcWySDPoTf9U8wlCZiOLgZvFrOU4AEqshBL1g8DO0RrPG1nKyKDKVi7Zf1QijzVCEKnKiNw33Cs0OBhqHBJbvX/g9M1syVwCEASYQIDAQABAoIBAQCoSlNP4v4zGUmz5/PXzvS0ml284PquptXODKnJbYmUv1+hB//+7WTg2ugb/LTIc5rqGG0718vljFfH3+nW5fNndWzmW4UVz6kbkIBKvIghQH+RwLv8JcuV5PlAUSD4TmKVawa7v0OVJ4bRkZlekgGQnTUDGsQWY5k4RjFcXtwEHlNfR0RsMptZpRj/AFASf/5pv1FjCxfAdImEq33Qnovtp/H5g2UDv2U0Ghh4BKq9wueTDQZTkAZs01ZBdwhhfT09hm1jLNch4lw6D7CsLTo0vvsYNpfGfXYnz7Wq//YrERQXakxrrON3/yPLFLgXgA0wRQzzdL7rr1chpWXZZsfhAoGBAPldFADhbzNkdKuOCsycexzQsGhmzBRhrmLoTuX8ckQOyk6Efwp3vUa00Aei+I43V6QvUamhG5xLD5CiVp7O/1g7J9HBBDtV/+oqR6DHb09zzV+Up2cg/d7PP/y6+WWv1EJw8vJ+OIt8zG13wno4q4onBolQUKdzwT3h1NTxV1JnAoGBAMO7rX/uLNn0NVydmnLBtY7nY+pA/YEw7xxRtG2Od9svoTsSWyx0uqVe9JmYF24+FS99Fs3HGpkPLU6fX+1eOJuwpm4ANHbh+WJKeKsJZ0kzKj0OX1gGhk/L+wkFcrH1bdjZrSNoHhRKqOtbQG8Ub3LaXwkeY/qz7G2ihvy8k0f3AoGANvJsmvkR0Mg88SR+erGsayANDyQ/qeoQTBkWcZUb9KUxrPWDLOJPocnr4RLnJCdLt5D5tKAcfmcHyf4LijjgiSeX0Z9veEet+YJRgDsR6vcAdSRDcvURG8ydM+pbVC3XXsCWuBpaCVp5jVGLV/Uuz2VkClPZQdy+4wzlIfsAG9cCgYEAj9KsHkDIvC5xt2gnQQONgUANbufqXq+yQ0DWL1psP6az9k/6pqPvMeygFuxm6x67OnXT98jBho5cYFVDi4ucVOpsoI+N/Xz2Xn8SQ7tZJbRu6QZSkL/amW/tOifCq3kICjuIuWecEpT5olce9YAylFZZVr36ARGMPNMcFUILhukCgYAnX6mA6AKhQMdtR8y8+dSNWw3YrxFbuR7vnbhCIT+7kVpXnInWJXzR8LidrshYvOdgBCJ3it5KCVy1pQL2FbF2iZ01rgRXyG2EwGg4OehQt4U5P1hrjogjH81NSj9h0LikC6jkU9sFixgTQoXwdl2KdDcCSbN9Asg59KORQsgpnw==";

			public static SymmetricSecurityKey GetSymmetricSecurityKey()
			{
				return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
			}
		}

		public string GenerateToken(ClaimsIdentity identity)
		{
			var now = DateTime.UtcNow;
			// создаем JWT-токен
			var jwt = new JwtSecurityToken(issuer: AuthOptions.ISSUER,
													  audience: AuthOptions.AUDIENCE,
													  notBefore: now,
													  claims: identity.Claims,
													  expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
													  signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
																								 SecurityAlgorithms.HmacSha256));

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
					new Claim(JWTClaimTypes.AppId, app.Id.Value),
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

		public TokenValidationParameters GetTokenValidationParameters()
			=> new TokenValidationParameters
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
