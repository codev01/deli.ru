using deli.data.Models.Contracts;
using deli.data.MongoDB.Common;
using deli.data.MongoDB.Services;
using deli.data.MongoDB.Services.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace deli.data.MongoDB.DataBases
{
	public class PrivateDB : MongoManager
	{
		public CollectionDependency Apps { get; set; }
		public CollectionDependency Accounts { get; set; }
		public PrivateDB(IDataBaseConnection dataBaseConnection, IServiceCollection services)
			: base(dataBaseConnection, services) { }

		protected override IServiceCollection RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<IAppService, AppService>();
			services.AddSingleton<IAccountService, AccountService>();
			return services;
		}

		protected override void RegisterCollections()
		{
			Apps = new CollectionDependency("apps", this);
			Accounts = new CollectionDependency("accounts", this);
		}
	}
}
