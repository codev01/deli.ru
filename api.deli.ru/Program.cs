using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

using api.deli.ru.Constants;
using api.deli.ru.Handlers;
using api.deli.ru.Managers;
using api.deli.ru.Middlewares;

using data.deli.ru.MongoDB.Serializers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

using Newtonsoft.Json.Converters;

namespace api.deli.ru
{
	public class Program
	{
		public static void Main(string[] args)
		{
			#region MongoDB registration
			// mongodb case type
			ConventionPack conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
			ConventionRegistry.Register("camelCase", conventionPack, t => true);
			// регистрация кастомного атрибута представляющий ObjectId в строковом представлении
			BsonSerializer.RegisterSerializer(new BsonObjectIdSerializer());
			BsonSerializer.RegisterSerializer(new BsonObjectIdArraySerializer());
			//BsonSerializer.RegisterSerializer(new BsonDurationsSerializer());
			#endregion

			// build web app
			WebApplicationBuilder builder = ServiceManager.BuildWebApplication(args);
			IServiceCollection services = builder.Services;

#if RELEASE
			// выставляем возможные стороки для обращения к api
			builder.WebHost.UseUrls(Properties.ApiConfigurations.UseUrls);
#endif

			WebApplication app = builder.Build();

			app.Use(async (HttpContext context, Func<Task> next) =>
			{
				Console.WriteLine($"\n{DateTime.Now} Запрос от {context.Connection.RemoteIpAddress}");
				Console.WriteLine($"\n{context.Request.Path.Value + context.Request.QueryString.Value}");
				await next.Invoke();
			});

			//////////////////////////////////////////////////////////////////////
			if (app.Environment.IsDevelopment())
			{
				//app.Map("/", () => Results.Redirect("/swagger"));
				//app.UseSwagger();
				//app.UseSwaggerUI();

				//app.UseExceptionHandler("/error-development");


				// показываем все объявленные сервисы 
				app.Map("/services", async context =>
				{
					var sb = new StringBuilder();
					sb.Append("<h1 align=\"center\" >" +
								"Все сервисы" +
							  "</h1>");
					sb.Append("<table>");
					sb.Append("<tr>" +
								"<th>Тип</th>	<th>Lifetime</th>	<th>Реализация</th>" +
							  "</tr>");
					foreach (var svc in services)
					{
						sb.Append("<tr>");
						sb.Append($"<td>{svc.ServiceType.FullName}</td>");
						sb.Append($"<td>{svc.Lifetime}</td>");
						sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
						sb.Append("</tr>");
					}
					sb.Append("</table>");
					context.Response.ContentType = "text/html;charset=utf-8";
					await context.Response.WriteAsync(sb.ToString());
				});
			}
			else
			{
				//app.UseExceptionHandler("/error");
			}

			// обработка необработаных исключений
			app.UseExceptionHandler(errorApp =>
			{
				errorApp.Run(async context =>
				{
					// Получаем исключение из контекста
					Exception exception = context.Features.Get<IExceptionHandlerFeature>().Error;

					// Логируем исключение
					Console.WriteLine(exception);

					// Отправляем ответ с ошибкой в формате JSON
					context.Response.ContentType = "application/json";
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					await context.Response.WriteAsync(new
					{
						error = "Произошла ошибка на сервере"
					}.ToString());
				});
			});

			// подключаем CORS
			// AllowAnyOrigin() -> указываем, что приложение может обрабатывать запросы из любых источников.
			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

			app.Map("/", () => Results.Redirect("/swagger"));
			app.UseSwagger();
			app.UseSwaggerUI();
			//////////////////////////////////////////////////////////////////////

			app.UseHttpsRedirection();

			// тут последовательность решает!
			app.UseRouting();
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseAuthentication();
			app.UseMiddleware<AuthMiddleware>();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});


			app.MapControllers();

			app.Run();
		}
	}
}