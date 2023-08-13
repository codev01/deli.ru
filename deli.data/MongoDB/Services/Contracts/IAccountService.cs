using deli.data.MongoDB.Models;

namespace deli.data.MongoDB.Services.Contracts
{
	public interface IAccountService : IMongoServiceBase<Account>
	{
		Task<Account> GetAccount(string user_name);
	}
}
