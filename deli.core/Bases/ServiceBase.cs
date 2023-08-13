using Microsoft.Extensions.DependencyInjection;

namespace deli.core.Bases
{
	public abstract class ServiceBase
	{
		private static IServiceProvider ServiceProvider { get; set; }
		private static IServiceCollection Services { get; set; }

		protected virtual IServiceCollection RegisterServices(IServiceCollection services)
			=> services;
		protected IServiceProvider BuildServices(IServiceCollection services)
		{
			Services = services;
			ServiceProvider = RegisterServices(services).BuildServiceProvider();
			return ServiceProvider;
		}

		public static T GetService<T>() where T : class
			=> ServiceProvider.GetService<T>() ?? throw new NullReferenceException("Запрашиваемый сервис не зарегистрирован");
	}
}