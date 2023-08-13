using System.Linq.Expressions;

using deli.core.Bases;
using deli.data.MongoDB;
using deli.data.MongoDB.Common;

using MongoDB.Driver;

using BsonObjectId = deli.data.MongoDB.Types.BsonObjectId;

namespace deli.data.Bases
{
	public abstract class MongoServiceBase<T> : ServiceBase, IMongoServiceBase<T>
		where T : IBsonObject
	{
		protected MongoManager MongoManager { get; }
		protected IMongoCollection<T> Collection { get; }

		public MongoServiceBase(CollectionDependency collection)
		{
			MongoManager = collection.MongoManager;
			Collection = collection.MongoManager.GetCollection<T>(collection.CollectionName);
		}

		public virtual async Task<IEnumerable<T>> GetById(string[] ids)
		{
			var filter = Builders<T>.Filter.In(x => x.Id, BsonObjectId.ConvertStrArray(ids));
			return await Collection.Find(filter).ToListAsync();
		}

		public virtual async void UpdateField<TField>(BsonObjectId id, Expression<Func<T, TField>> field, TField value)
		{
			var filter = Builders<T>.Filter.Eq(p => p.Id, id);
			var update = Builders<T>.Update.Set(field, value);
			await Collection.UpdateOneAsync(filter, update);
		}

		public virtual async void AddDocument(T document)
			=> await Collection.InsertOneAsync(document);
	}
}
