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
			BsonSerializer.RegisterSerializer(new BsonDurationsSerializer());
			#endregion

			// build web app
			WebApplicationBuilder builder = ServiceManager.BuildWebApplication(args);
			IServiceCollection services = builder.Services;

#if RELEASE
			// выставляем возможные стороки для обращения к api
			builder.WebHost.UseUrls(Properties.ApiConfigurations.UseUrls);
#endif
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddScheme<DefaultSchemeOptions, DefaultSchemeHandler>(JwtBearerDefaults.AuthenticationScheme,
			options => { });

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policies.TenantAndLandlord, policy =>
				{
					policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					policy.RequireRole(Roles.Tenant, Roles.Landlord);
				});
			});

			#region Swagger Configuration
			services.AddSwaggerGen(swagger =>
			{
				//This is to generate the Default UI of Swagger Documentation
				swagger.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "deli.ru",
					Description = "WWS"
				});

				var securityScheme = new OpenApiSecurityScheme()
				{
					Name = Headers.TokenHeaderName,
					Type = SecuritySchemeType.ApiKey,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token",
				};

				// To Enable authorization using Swagger (JWT)
				swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = JwtBearerDefaults.AuthenticationScheme
							}
						},
						new string[] { }
					}
				});

				// Интеграция XML-комментариев с Swagger
				var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
				swagger.IncludeXmlComments(xmlCommentsPath);

				// Добавление фильтра операции для автоматического добавления заголовка Authorization
				//swagger.OperationFilter<SwaggerAuthorizationHeaderFilter>();
			});
			#endregion

			// CORS
			services.AddCors();

			// Add services to the container.
			//services.AddControllers(options =>
			//{
			//	options.Filters.Add<AuthFilter>();
			//});

			// Игнорирование свойств при сериализации
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					// палитика имён свойств
					options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
					// игнорирование свойств со значением null
					options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
					// игнорирование свойств со значением по умолчанию
					options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
					// сериализаторы BsonObjectId
					options.JsonSerializerOptions.Converters.Add(new JsonObjectIdSerializer());
					options.JsonSerializerOptions.Converters.Add(new JsonObjectIdArraySerializer());
					options.JsonSerializerOptions.Converters.Add(new JsonDurationsSerializer());
				});


			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

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