using System.Net;
using System.Text;

using deli.api.Managers;
using deli.api.Middlewares;

using deli.data.MongoDB.Serializers;

using Microsoft.AspNetCore.Diagnostics;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace deli.api
{
	public class _Program
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
			WebApplicationBuilder builder = ApiManager.BuildWebApplication(args);
			IServiceCollection services = builder.Services;

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

			// обработка необработанных исключений
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
			app.UseAuthentication();
			app.UseMiddleware<AuthMiddleware>();
			app.UseAuthorization();
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			app.MapControllers();

			app.Run();
		}
	}
}