using data.deli.ru.MongoDB.Models;
using System.Security.Claims;

namespace data.deli.ru.MongoDB.Services.Contracts
{
	public interface IAppService : IMongoServiceBase<App> { }
}
