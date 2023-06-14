using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using static auth.deli.ru.Controllers.AuthController;

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

		private static List<Person> People = new List<Person>
		{
			new Person { UserName = "int", Email="admin@gmail.com", Password="00000", Role = "landlord" },
			new Person { UserName = "rx1310", Email="analitik@gmail.com", Password="11111", Role = "tenant" },
			new Person { UserName = "codev01", Email="developer@gmail.com", Password="55987", Role = "landlord" },
			new Person { UserName = "tomodachi", Email="developer@gmail.com", Password="54321", Role = "lendlord" },
			new Person { UserName = "gambit", Email="qwerty@gmail.com", Password="00001", Role = "tenant" }
		};
		private static List<App> Apps = new List<App>
		{
			new App{ Id = 111111111, ClientSecret = "qwertyuiop", Scopes = new[]{ "natural" }, Version = "1.2" },
			new App{ Id = 222222222, ClientSecret = "asdfghjkl", Scopes = new[]{ "legal" }, Version = "1.0.2" },
			new App{ Id = 222222222, ClientSecret = "asdfghjkl", Scopes = new[]{ "natural", "legal" }, Version = "1"},
			new App{ Id = 222222222, ClientSecret = "asdfghjkl", Scopes = new[]{ "legal", "elevatedUser" }, Version = "special"}
		};

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
		private string GenerateToken(App app, Person person)
		{
			var appIdentity = GetAppIdentity(app);
			var userIdentity = GetUserIdentity(person);
			var identity = new ClaimsIdentity(appIdentity.Claims.Union(userIdentity.Claims));  

			return null;
		}

		private ClaimsIdentity GetAppIdentity(App app)
		{
			App? validApp = Apps.FirstOrDefault(x => x.Id == app.Id && x.ClientSecret == app.ClientSecret);
			if (validApp != null)
			{
				var claims = new List<Claim>
				{
					new Claim("app_id", validApp.Id.ToString()),
					new Claim("app_version", validApp.Version)
				};
				foreach (string scope in validApp.Scopes)
					claims.Add(new Claim("app_scope", scope));

				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
				return claimsIdentity;
			}
			else
				// если пользователя не найдено
				throw new ArgumentException("Invalid username or password.");
		}


		private ClaimsIdentity GetUserIdentity(Person person)
		{
			Person? validPerson = People.FirstOrDefault(x => x.UserName == person.UserName && 
													    x.Password == person.Password);
			if (validPerson != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, validPerson.UserName),
					new Claim(ClaimTypes.Email, validPerson.Email),
					new Claim(ClaimTypes.Role, validPerson.Role),
					new Claim(ClaimTypes.MobilePhone, validPerson.MobilePhone)
				};
				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
				return claimsIdentity;
			}
			else
				// если пользователя не найдено
				throw new ArgumentException("Invalid username or password.");
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
			public string ClientSecret { get; set; }
			public string[] Scopes { get; set; }
			public string Version { get; set; }
		}
	}
}
