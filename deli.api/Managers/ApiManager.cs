using System.Net;
using System.Reflection;
using System.Text.Json;

using deli.api.ConfigModels;
using deli.api.Constants;
using deli.api.Handlers;
using deli.api.Services;
using deli.api.Services.Contracts;
using deli.core.Bases;
using deli.data;
using deli.data.MongoDB.Serializers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace deli.api.Managers
{
	public class ApiManager : Service_Base
	{
		private static ApiManager Current { get; } = new();
		private static ConfigurationManager ConfigurationManager { get; set; }

		protected override IServiceCollection RegisterServices(IServiceCollection services)
		{
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

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			// Настройки сериализации Json
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					// политика имён свойств
					options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
					// игнорирование свойств со значением null
					//options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
					// игнорирование свойств со значением по умолчанию
					//options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
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

			services.AddSingleton<DatabaseManager_>(new DatabaseManager_(configs.DatabaseConnections, services));

			services.AddSingleton<IFileService, LocalFileService>();

			services.AddSingleton<IAuthService, AuthService>();


			return services;
		}

		public static WebApplicationBuilder BuildWebApplication(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			ConfigurationManager = builder.Configuration;
			Current.BuildServices(builder.Services);

			var configs = GetConfig<ApiConfigurations>();

			if (configs.Settings.IsHttpsEnable)
			{
				// выставляем возможные строки для обращения к api
				builder.WebHost.UseUrls(configs.EndPoints.Https.Urls);
				builder.WebHost.ConfigureKestrel((context, options) =>
				{
					options.Listen(IPAddress.Any, 443, listenOptions =>
					{
						listenOptions.UseHttps(configs.EndPoints.Https.Certificate.Path, configs.EndPoints.Https.Certificate.Password);
					});
				});
			}
			else
				// выставляем возможные строки для обращения к api
				builder.WebHost.UseUrls(configs.EndPoints.Http.Urls);

			return builder;
		}

		public static T GetConfig<T>() where T : class
			=> ConfigurationManager.GetSection(typeof(T).Name).Get<T>() ?? throw new NullReferenceException("Null при десериализации конфигураций");
	}
}
