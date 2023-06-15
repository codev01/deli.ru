﻿using System.Net;
using System.Reflection;
using System.Text;

using api.deli.ru.Filters;
using api.deli.ru.Managers;
using api.deli.ru.Middlewares;
using data.deli.ru.MongoDB.Serializers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

using NuGet.Common;

using static api.deli.ru.Middlewares.AuthMiddleware;

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
			#endregion

			// build web app
			WebApplicationBuilder builder = ServiceManager.BuildWebApplication(args);
			IServiceCollection services = builder.Services;

#if RELEASE
			// выставляем возможные стороки для обращения к api
			builder.WebHost.UseUrls(Properties.ApiConfigurations.UseUrls);
#endif
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
				AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
				{
					var key = AuthOptions.GetSymmetricSecurityKey();

					// если равно false, то SSL при отправке токена не используется.
					// Однако данный вариант установлен только дя тестирования.
					// лучше использовать передачу данных по протоколу https.
					config.RequireHttpsMetadata = false;

					
					//параметры валидации токена
					config.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true, // Валидация источника токена
						ValidateAudience = true, // Валидация получателя токена
						ValidateLifetime = true, // Валидация срока действия токена
						ValidateIssuerSigningKey = true, // Валидация ключа подписи токена
						
						// Указание верных источника и получателя токена
						ValidIssuer = AuthOptions.ISSUER,
						ValidAudience = AuthOptions.AUDIENCE,

						// Указание параметров валидации, например, секретного ключа и других
						// Вам нужно будет заменить "YOUR_SECRET_KEY" на ваш секретный ключ
						IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey() // The same key as the one that generate the token
					};
				});

			// Конфигурация авторизации по ролям
			services.AddAuthorization(options =>
			{
				options.AddPolicy("tenant", policy =>
				{
					policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					policy.RequireRole("tenant", "landlord");
				});
				options.AddPolicy("landlord", policy =>
				{
					policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					policy.RequireRole("landlord");
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
					Name = "Authorization",
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
			services.AddControllers();

			// Игнорирование свойств при сериализации
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					// игнорирование свойств со значением null
					options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
					// игнорирование свойств со значением по умолчанию
					options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
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
			// Configure the HTTP request pipeline.
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
			// AllowAnyOrigin() ->  указываем, что приложение может обрабатывать запросы от приложений по любым адресам.
			app.UseCors(builder => builder.AllowAnyOrigin());

			app.Map("/", () => Results.Redirect("/swagger"));
			app.UseSwagger();
			app.UseSwaggerUI();
			//////////////////////////////////////////////////////////////////////

			// ловит исключения
			// про конвееры и middleware: https://metanit.com/sharp/aspnet5/2.4.php
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseMiddleware<AuthMiddleware>();
			// обработка ошибок HTTP 
			//app.UseStatusCodePagesWithRedirects("/");

			//app.UseCors("CorsPolicy");
			//app.UseRouting();

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}