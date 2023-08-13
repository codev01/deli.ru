using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace deli.api.Controllers
{
	//[Authorize]
	//[Route("[controller]/[action]")]
	//[ApiController]
	public class TestController : ControllerBase
	{
		// атрибут [Route("[controller]/[action]")]
		// будет представлять собой примерно такую ссылку
		// https://192.168.0.105:7045/[controller]/[action]/...

		// атрибут [HttpGet("{id}")] задаёт имя [action] 
		// пустой атрибут [HttpPost] будет означать что в качестве [action] будет имя метода
		// "{id}" указывает в качестве имени параметр
		// https://192.168.0.105:7045/[controller]/[action]/{id}



		// Это список атрибутов обозначающие тип обработчика
		// [HttpGet] - получить
		// [HttpPost] - отправить
		// [HttpPut] - изменить
		// [HttpDelete] - удалить
		// GET: этот метод является безопасным и идемпотентным.Обычно используется для извлечения информации и не имеет побочных эффектов.
		// POST: этот метод не является ни безопасным, ни идемпотентным. Этот метод наиболее широко используется для создания ресурсов.
		// PUT: этот метод является идемпотентным. Вот почему лучше использовать этот метод вместо POST для обновления ресурсов.Избегайте использования POST для обновления ресурсов.
		// DELETE: как следует из названия, этот метод используется для удаления ресурсов. Но этот метод не является идемпотентным для всех запросов.
		// OPTIONS: этот метод не используется для каких-либо манипуляций с ресурсами. Но он полезен, когда клиент не знает других методов, поддерживаемых для ресурса, и используя этот метод, клиент может получить различное представление ресурса.
		// HEAD: этот метод используется для запроса ресурса c сервера. Он очень похож на метод GET, но HEAD должен отправлять запрос и получать ответ только в заголовке.Согласно спецификации HTTP, этот метод не должен использовать тело для запроса и ответа.



		// Коды ошибок
		// 200 OK — это ответ на успешные GET, PUT, PATCH или DELETE.Этот код также используется для POST, который не приводит к созданию.
		// 201 Created — этот код состояния является ответом на POST, который приводит к созданию.
		// 204 Нет содержимого. Это ответ на успешный запрос, который не будет возвращать тело (например, запрос DELETE)
		// 304 Not Modified — используйте этот код состояния, когда заголовки HTTP-кеширования находятся в работе
		// 400 Bad Request — этот код состояния указывает, что запрос искажен, например, если тело не может быть проанализировано
		// 401 Unauthorized — Если не указаны или недействительны данные аутентификации.Также полезно активировать всплывающее окно auth, если приложение используется из браузера
		// 403 Forbidden — когда аутентификация прошла успешно, но аутентифицированный пользователь не имеет доступа к ресурсу
		// 404 Not found — если запрашивается несуществующий ресурс
		// 405 Method Not Allowed — когда запрашивается HTTP-метод, который не разрешен для аутентифицированного пользователя
		// 410 Gone — этот код состояния указывает, что ресурс в этой конечной точке больше не доступен.Полезно в качестве защитного ответа для старых версий API
		// 415 Unsupported Media MongoType.Если в качестве части запроса был указан неправильный тип содержимого
		// 422 Unprocessable Entity — используется для проверки ошибок
		// 429 Too Many Requests — когда запрос отклоняется из-за ограничения скорости



		//[HttpGet("{userName}")]
		//public async Task<string> GetUserPage(string userName)
		//{
		//	//var userName = this.RequestContext.Principal.Identity.Name;
		//	return String.Format("Hello, {0}.", userName);
		//	//return "dsfsdg";
		//}
		// Обязательные параметры
		// Инфо: https://metanit.com/sharp/aspnet5/8.5.php
		[Bind("Name")]
		// Можно так указать обязательные параметры,
		// таким образом они будут обязательны во всех методах где используется этот класс
		public class User
		{
			public int Id { get; set; }

			[BindRequired] // Делает параметр обязательным
			public string Name { get; set; }

			public int Age { get; set; }
			[BindNever] // Делает параметр необязательным
			public bool HasRight { get; set; }
		}
		// Можно и так указать обязательные параметры
		// public IActionResult AddUser([Bind("Name", "Age", "HasRight")] User user)
		public IActionResult AddUser(User user)
		{
			if (ModelState.IsValid) // проверяем приняты ли обязательные параметры
			{
				string userInfo = $"Id: {user.Id}  Name: {user.Name}  Age: {user.Age}  HasRight: {user.HasRight}";
				return Content(userInfo);
			}
			return Content($"Количество ошибок: {ModelState.ErrorCount}");
		}


		// Вызвать http исключение можно таким образом
		//throw new HttpResponseException(StatusCode.NotFound);


		// GET: api/TodoItems/5
		//[HttpGet]
		//public Response<string> GetTodoItem(long id)
		//{
		//	return this.TryInvoke(() =>
		//	{
		//		//Task.Delay(10000).Wait();
		//		//var w = 5;
		//		//var qw = w / 0;

		//		return "GetTodoItem";
		//	});

		//	//return "codev01";
		//}

		//[HttpGet]
		//public async Task<Response<string>> qwe()
		//{
		//	return await this.TryInvokeAsync(() =>
		//	{
		//		Task.Delay(10000).Wait();
		//		return "qwe";
		//	});
		//}

		// GET api/<AccountController>/5
		//[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "Hello ASP.NET";
		}

		// POST api/<AccountController>
		[HttpPost]
		public void Post(MyClass my)
		{
		}

		// PUT api/<AccountController>/5
		//[HttpPut("{id}")]
		public void Put(int id, string value)
		{
		}

		// DELETE api/<AccountController>/5
		//[HttpDelete("{id}")]
		public void Delete(int id)
		{
#nullable disable
#nullable restore

			string name = null;
			PrintUpper(name!);

			void PrintUpper(string text)
			{
				if (text == null) Console.WriteLine("null");
				else Console.WriteLine(text.ToUpper());
			}

		}
	}


#nullable disable
	public class MyClass : Valid
	{
		public int id { get; set; }
		public string name { get; set; }
	}

	public class Valid
	{
		public string Token { get; set; }
		public string V { get; set; }
	}
}
