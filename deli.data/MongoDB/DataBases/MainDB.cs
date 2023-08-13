using deli.data.Models.Contracts;
using deli.data.MongoDB.Common;
using deli.data.MongoDB.Services;
using deli.data.MongoDB.Services.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace deli.data.MongoDB.DataBases
{
	public class MainDB : MongoManager
	{
		public CollectionDependency Announcements { get; set; }
		public CollectionDependency Products { get; set; }
		public MainDB(IDataBaseConnection dataBaseConnection, IServiceCollection services)
			: base(dataBaseConnection, services) { }

		protected override IServiceCollection RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<ICategoryService, CategoryService>();
			services.AddSingleton<IProductService, ProductService>();
			services.AddSingleton<IAnnouncementService, AnnouncementService>();

			return services;
		}

		protected override void RegisterCollections()
		{
			Announcements = new CollectionDependency("announcements", this);
			Products = new CollectionDependency("products", this);
		}
	}
}
