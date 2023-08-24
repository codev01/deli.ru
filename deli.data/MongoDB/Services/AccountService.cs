using deli.data.Bases;
using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;

using MongoDB.Driver;

namespace deli.data.MongoDB.Services
{
	public class AccountService : Mongo_Service_Base<Account>, IAccountService
	{
		public AccountService(DatabaseManager_ dataBaseManager) : base(dataBaseManager.Private.Accounts) { }

		public async Task<Account> GetAccount(string user_name) => await Collection
			.Find(a => a.UserName == user_name)
			.FirstOrDefaultAsync();
	}
}
