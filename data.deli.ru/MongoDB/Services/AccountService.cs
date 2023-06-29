using data.deli.ru.Bases;
using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;

using MongoDB.Bson;
using MongoDB.Driver;

using static MongoDB.Driver.WriteConcern;

namespace data.deli.ru.MongoDB.Services
{
	public class AccountService : MongoServiceBase<Account>, IAccountService
	{
		public AccountService(DataBaseManager dataBaseManager) : base(dataBaseManager.Private, "accounts") { }

		public async Task<Account> GetAccount(string user_name) => await Collection
			.Find(a => a.UserName == user_name)
			.FirstOrDefaultAsync();
    }
}
