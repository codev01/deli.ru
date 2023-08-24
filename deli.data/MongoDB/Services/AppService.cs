using deli.data.Bases;
using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;

namespace deli.data.MongoDB.Services
{
	public class AppService : Mongo_Service_Base<App>, IAppService
	{
		public AppService(DatabaseManager_ dataBaseManager) : base(dataBaseManager.Private.Apps) { }
	}
}
