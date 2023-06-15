using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace api.deli.ru.Middlewares
{
	public class AuthMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			context.Request.Headers["Authorization"] = "Bearer " + context.Request.Headers["Authorization"];
			await _next(context);
		}
		public static bool ValidateToken(string authToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = GetValidationParameters();

			SecurityToken validatedToken;
			tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			var userId = jwtToken.Claims.First(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;

			if (userId is not null)
			{
				return true;
			}
			else
			{
				return false;

			}

		}
		private static TokenValidationParameters GetValidationParameters()
		{
			return new TokenValidationParameters()
			{
				ValidIssuer = AuthOptions.ISSUER,
				ValidAudience = AuthOptions.AUDIENCE,				
				IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey() // The same key as the one that generate the token
			};
		}
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
	}
}
