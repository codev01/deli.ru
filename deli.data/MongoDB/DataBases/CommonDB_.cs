using deli.data.Models.Contracts;
using deli.data.MongoDB.Common;
using deli.data.MongoDB.Services;
using deli.data.MongoDB.Services.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace deli.data.MongoDB.DataBases
{
	public class CommonDB_ : _MongoManager
	{
		public CollectionDependency Cities { get; set; }
		public CollectionDependency Categories { get; set; }
		public CommonDB_(IDataBaseConnection_ dataBaseConnection, IServiceCollection services)
			: base(dataBaseConnection, services) { }

		protected override IServiceCollection RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<ICityService, CityService>();
			return services;
		}

		protected override void RegisterCollections()
		{
			Cities = new CollectionDependency("cities", this);
			Categories = new CollectionDependency("categories", this);
		}
	}
}
