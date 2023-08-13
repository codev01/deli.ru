using deli.data.Bases;
using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;

namespace deli.data.MongoDB.Services
{
	public class AppService : MongoServiceBase<App>, IAppService
	{
		public AppService(DataBaseManager dataBaseManager) : base(dataBaseManager.Private.Apps) { }
	}
}
