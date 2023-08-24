using deli.data.MongoDB.Models;

namespace deli.data.MongoDB.Services.Contracts
{
	public interface IAccountService : IMongo_Service_Base<Account>
	{
		Task<Account> GetAccount(string user_name);
	}
}
