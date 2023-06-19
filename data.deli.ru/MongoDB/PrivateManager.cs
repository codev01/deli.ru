using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using data.deli.ru.Contracts;
using data.deli.ru.MongoDB.Bases;
using data.deli.ru.MongoDB.Services;
using data.deli.ru.MongoDB.Services.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace data.deli.ru.MongoDB
{
	public class PrivateManager : MongoManager
	{
		public PrivateManager(IDataBaseConnection dataBaseConnection, IServiceCollection services)
			: base(dataBaseConnection)
		{
			RegisterServices(services);
		}

		protected override IServiceCollection RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<IAppService, AppService>();
			services.AddSingleton<IAccountService, AccountService>();
			return services;
		}
	}
}
