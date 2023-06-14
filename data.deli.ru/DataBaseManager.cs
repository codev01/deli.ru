using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using data.deli.ru.MongoDB;
using data.deli.ru.MongoDB.Bases;

using Microsoft.Extensions.DependencyInjection;

namespace data.deli.ru
{
	public class DataBaseManager
	{
		public MainManager Main { get; }
		public CommonManager Common { get; }
		public OtherDatabase OtherDatabase { get; }

        public DataBaseManager(DatabaseConnections databaseConnections, IServiceCollection services)
        {
			Main = new MainManager(databaseConnections.Main, services);
			Common = new CommonManager(databaseConnections.Common, services);
		}
    }
}
