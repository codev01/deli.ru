using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using static auth.deli.ru.Controllers.AuthController;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace auth.deli.ru.Controllers
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

	[ApiController]
	[Route("[controller].[action]")]
	public class AuthController : ControllerBase 
	{

		private static List<Person> People = new List<Person>
		{
			new Person ("int", "00000") { Email="admin@gmail.com", Role = "landlord" },
			new Person ("rx1310", "11111") { Email = "analitik@gmail.com", Role = "tenant" },
			new Person ("codev01", "55987") { Email = "developer@gmail.com", Role = "landlord" },
			new Person ("tomodachi", "54321") { Email = "developer@gmail.com", Role = "lendlord" },
			new Person ("gambit", "00001") { Email = "qwerty@gmail.com", Role = "tenant" }
		};
		private static List<App> Apps = new List<App>
		{
			new App(111111111, "qwertyuiop") { Scopes = new[]{ "natural" }, Version = "1.2" },
			new App(222222222, "asdfghjkl") { Scopes = new[]{ "legal" }, Version = "1.0.2" },
			new App(333333333, "asdfghjkl") { Scopes = new[]{ "natural", "legal" }, Version = "1"},
			new App(444444444, "asdfghjkl") { Scopes = new[]{ "legal", "elevatedUser" }, Version = "special"}
		};

		[HttpGet]
		public IActionResult Login(int app_id, string client_secret, string user_name, string password)
		{
			string token = string.Empty;
			try
			{
				token = GenerateToken(new App(app_id, client_secret), new Person(user_name, password));
				return Ok(new { user_name = user_name, token = token });
			}
			catch (Exception e)
			{
				return Unauthorized(new { errorText = e.Message, error_obj = JsonConvert.SerializeObject(e) });
			}
		}
		private string GenerateToken(App app, Person person)
		{
			var appIdentity = GetAppIdentity(app);
			var userIdentity = GetUserIdentity(person);
			var identity = new ClaimsIdentity(appIdentity.Claims.Union(userIdentity.Claims));
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
				throw new ArgumentException("Invalid app_id or client_secret.");
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
					new Claim(ClaimTypes.MobilePhone, $"{validPerson.MobilePhone}")
				};
				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
				return claimsIdentity;
			}
			else
				// если пользователя не найдено
				throw new ArgumentException("Invalid username or password.");
		}

		public class Person
		{
			public string UserName { get; set; }
			public string Password { get; set; }
			public string Role { get; set; }
			public string Email { get; set; }
			public string MobilePhone { get; set; }
            public Person(string user_name, string password)
            {
                UserName = user_name;
				Password = password;
            }
        }
		public class App
		{
			public int Id { get; set; }
			public string ClientSecret { get; set; }
			public string[] Scopes { get; set; }
			public string Version { get; set; }
            public App(int app_id, string client_secret)
            {
                Id = app_id;
				ClientSecret = client_secret;
            }
        }
	}
}
