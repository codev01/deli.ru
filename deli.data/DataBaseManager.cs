using deli.data.MongoDB.DataBases;

using Microsoft.Extensions.DependencyInjection;

namespace deli.data
{
	public class DataBaseManager
	{
		public MainDB Main { get; }
		public CommonDB Common { get; }
		public PrivateDB Private { get; }
		public OtherDatabase OtherDatabase { get; }

		public DataBaseManager(DatabaseConnections databaseConnections, IServiceCollection services)
		{
			Main = new MainDB(databaseConnections.Main, services);
			Common = new CommonDB(databaseConnections.Common, services);
			Private = new PrivateDB(databaseConnections.Private, services);
		}
	}
}
