using data.deli.ru.MongoDB;

using Microsoft.Extensions.DependencyInjection;

namespace data.deli.ru
{
	public class DataBaseManager
	{
		public MainManager Main { get; }
		public CommonManager Common { get; }
		public PrivateManager Private { get; }
		public OtherDatabase OtherDatabase { get; }

		public DataBaseManager(DatabaseConnections databaseConnections, IServiceCollection services)
		{
			Main = new MainManager(databaseConnections.Main, services);
			Common = new CommonManager(databaseConnections.Common, services);
			Private = new PrivateManager(databaseConnections.Private, services);
		}
	}
}
