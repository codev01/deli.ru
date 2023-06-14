using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace auth.deli.ru.Controllers
{
	public class AuthOptions
	{
		public const string ISSUER = "auth.deli.ru"; // издатель токена
		public const string AUDIENCE = "webClient.deli.ru"; // потребитель токена
		const string KEY = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1!123";   // ключ для шифрации
		public const int LIFETIME = 43200; // время жизни токена - 1 минута
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
		}
	}

	[ApiController]
	[Route("[controller].[action]")]
	public class AuthController : ControllerBase
	{

		[HttpGet]
		public IActionResult Login(string username, string password)
		{

			var identity = GetIdentity(username, password);
			if (identity == null)
			{
				return BadRequest(new { errorText = "Invalid username or password." });
			}

			var now = DateTime.UtcNow;
			// создаем JWT-токен
			var jwt = new JwtSecurityToken(
					issuer: AuthOptions.ISSUER,
					audience: AuthOptions.AUDIENCE,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), 
															   SecurityAlgorithms.HmacSha256));

			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				username = identity.Name,
				access_token = encodedJwt
			};

			return Ok(response);
		}



		private static ClaimsIdentity GetIdentity(string username, string password)
		{
			Person person = people.FirstOrDefault(x => x.UserName == username && x.Password == password);
			if (person != null)
			{
				var claims = new List<Claim>
				{
					new Claim(JwtRegisteredClaimNames.UniqueName, person.UserName),
					new Claim(ClaimTypes.Email, person.Email),
					new Claim(ClaimTypes.Role, person.Role),
					new Claim(ClaimTypes.MobilePhone, person.MobilePhone),
					//new Claim(ClaimTypes., person.MobilePhone)
				};
				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
				return claimsIdentity;
			}

			// если пользователя не найдено
			return null;
		}
		private static List<Person> people = new List<Person>
		{
			new Person { UserName = "int", Email="admin@gmail.com", Password="00000", Role = "admin" },
			new Person { UserName = "rx1310", Email="analitik@gmail.com", Password="11111", Role = "analitik" },
			new Person { UserName = "codev01", Email="developer@gmail.com", Password="55987", Role = "dev" },
			new Person { UserName = "tomodachi", Email="developer@gmail.com", Password="54321", Role = "dev" },
			new Person { UserName = "gambit", Email="qwerty@gmail.com", Password="00001", Role = "user" }
		};
		private static List<App> Apps = new List<App>
		{
			new App{ Id = 111111111, Secret = "qwertyuiop" },
			new App{ Id = 222222222, Secret = "asdfghjkl" }
		};
		public class Person
		{
			public string UserName { get; set; }
			public string Password { get; set; }
			public string Role { get; set; }
			public string Email { get; set; }
			public string MobilePhone { get; set; }
		}
		public class App
		{
			public int Id { get; set; }
			public string Secret { get; set; }
		}

		private static string GenerateToken(string username, string password)
		{
			//var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			//var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			//var secToken = new JwtSecurityToken(
			//	signingCredentials: credentials,
			//	issuer: "Sample",
			//	audience: "Sample",
			//	claims: new[]
			//	{
			//	new Claim(JwtRegisteredClaimNames.Sub, "meziantou")
			//	},
			//	expires: DateTime.UtcNow.AddDays(1));

			//var handler = new JwtSecurityTokenHandler();
			//return handler.WriteToken(secToken);
			return null;
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
	}
}
