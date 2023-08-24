using deli.data.Models.Contracts;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;
using MongoDB.Driver;

namespace deli.data.MongoDB
{
	public abstract class _MongoManager : MongoDatabaseBase
	{
		public IMongoDatabase Database { get; }
		public override IMongoClient Client { get; }
		public override DatabaseNamespace DatabaseNamespace { get; }
		public override MongoDatabaseSettings Settings { get; }

		public _MongoManager(IDataBaseConnection_ dataBaseConnection, IServiceCollection services)
		{
			_isInit = true;
			Client = new MongoClient(dataBaseConnection.ConnectionString);
			Database = Client.GetDatabase(dataBaseConnection.DataBaseName);
			RegisterServices(services);
			RegisterCollections();
		}

		protected abstract IServiceCollection RegisterServices(IServiceCollection services);
		protected abstract void RegisterCollections();

		public override IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
		{
			return Database.GetCollection<TDocument>(name);
		}

		public override Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public override Task DropCollectionAsync(string name, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public override Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public override Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public override Task<TResult> RunCommandAsync<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		private static bool _isRegisteredServices = false;
		private static bool _isInit = false;
	}
}
