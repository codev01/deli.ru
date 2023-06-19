using api.deli.ru.ConfigModels;
using api.deli.ru.Filters;
using api.deli.ru.Services;
using api.deli.ru.Services.Contracts;

using data.deli.ru;
using data.deli.ru.Contracts;
using data.deli.ru.MongoDB;

using Microsoft.AspNetCore.Authorization;

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
