﻿using data.deli.ru.MongoDB.Models;

namespace data.deli.ru.MongoDB.Services.Contracts
{
	public interface IAccountService : IMongoServiceBase<Account>
	{
		Task<Account> GetAccount(string user_name);
	}
}
