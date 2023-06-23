using data.deli.ru.Contracts;
using data.deli.ru.MongoDB.Bases;
using data.deli.ru.MongoDB.Services;
using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace data.deli.ru.MongoDB
{
	public class CommonManager : MongoManager
	{
		public CommonManager(IDataBaseConnection dataBaseConnection, IServiceCollection services)
			: base(dataBaseConnection)
		{
			RegisterServices(services);
		}

		protected override IServiceCollection RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<ICityService, CityService>();
			return services;
		}
	}
}
