using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using data.deli.ru.Bases;
using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;

namespace data.deli.ru.MongoDB.Services
{
	public class AccountService : MongoServiceBase<Account>, IAccountService
	{
        public AccountService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main, "accounts") { }
	}
}
