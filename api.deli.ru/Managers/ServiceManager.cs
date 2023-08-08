using System.Reflection;
using System.Text.Json;

using api.deli.ru.ConfigModels;
using api.deli.ru.Constants;
using api.deli.ru.Handlers;
using api.deli.ru.Services;
using api.deli.ru.Services.Contracts;
using data.deli.ru;
using data.deli.ru.MongoDB.Serializers;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.OpenApi.Models;

namespace api.deli.ru.Managers
{
	public static class ServiceManager
	{
		private static IServiceProvider ServiceProvider { get; set; }
		private static ConfigurationManager ConfigurationManager { get; set; }

		private static IServiceCollection RegisterServices(IServiceCollection services)
		{
			if (!_isRegisteredServices)
			{
				_isRegisteredServices = true;

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


				services.AddEndpointsApiExplorer();
				services.AddSwaggerGen();
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
						//options.JsonSerializerOptions.Converters.Add(new JsonDurationsSerializer());
					});

				services.AddSingleton<IServiceCollection>(services);

				// сайт в котором есть инфа о том как динамически читать файл конфигураций
				// https://edi.wang/post/2019/1/5/auto-refresh-settings-changes-in-aspnet-core-runtime
				//services.AddTransient<ApiConfigurations>((s) => GetConfig<ApiConfigurations>()!);

				var configs = GetConfig<ApiConfigurations>();

				services.AddSingleton(configs);

				services.AddSingleton<DataBaseManager>(new DataBaseManager(configs.DatabaseConnections, services));

				services.AddSingleton<IFileService, LocalFileService>();

				services.AddSingleton<IAuthService, AuthService>();
			}

			return services;
		}

		public static WebApplicationBuilder BuildWebApplication(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			ConfigurationManager = builder.Configuration;
			ServiceProvider = RegisterServices(builder.Services).BuildServiceProvider();
			return builder;
		}

		public static T GetConfig<T>() where T : class
			=> ConfigurationManager.GetSection(typeof(T).Name).Get<T>() ?? throw new NullReferenceException("Null при десериализации конфигураций");

		public static T GetService<T>() where T : class
			=> ServiceProvider.GetService<T>() ?? throw new NullReferenceException("Зпрашиваемый сервис не проинициализирован");

		private static bool _isRegisteredServices = false;
	}
}
