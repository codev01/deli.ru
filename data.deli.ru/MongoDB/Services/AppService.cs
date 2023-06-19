using data.deli.ru.Bases;
using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;

namespace data.deli.ru.MongoDB.Services
{
	public class AppService : MongoServiceBase<App>, IAppService
	{
		public AppService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main, "apps") { }
	}
}
