using deli.data.MongoDB.DataBases;

using Microsoft.Extensions.DependencyInjection;

namespace deli.data
{
	public class DatabaseManager_
	{
		public MainDB_ Main { get; }
		public CommonDB_ Common { get; }
		public PrivateDB_ Private { get; }
		public OtherDatabase_ OtherDatabase { get; }

		public DatabaseManager_(DatabaseConnections_ databaseConnections, IServiceCollection services)
		{
			Main = new MainDB_(databaseConnections.Main, services);
			Common = new CommonDB_(databaseConnections.Common, services);
			Private = new PrivateDB_(databaseConnections.Private, services);
		}
	}
}
