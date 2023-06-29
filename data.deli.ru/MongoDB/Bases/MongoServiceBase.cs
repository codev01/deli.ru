using System.Linq.Expressions;

using data.deli.ru.MongoDB.Bases;
using data.deli.ru.MongoDB.Models;

using MongoDB.Driver;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.Bases
{
	public abstract class MongoServiceBase<T> where T : IBsonObject
	{
		protected MongoManager MongoManager { get; }
		protected IMongoCollection<T> Collection { get; }

		public MongoServiceBase(MongoManager mongoManager, string collectionName)
		{
			MongoManager = mongoManager;
			Collection = mongoManager.GetCollection<T>(collectionName);
		}

		public virtual async Task<IEnumerable<T>> GetById(string[] ids)
		{
			var filter = Builders<T>.Filter.In(x => x.Id, BsonObjectId.ConvertStrArray(ids));
			return await Collection.Find(filter).ToListAsync();
		}

		public virtual async void UdateField<TField>(BsonObjectId id, Expression<Func<T, TField>> field, TField value)
		{
			var filter = Builders<T>.Filter.Eq(p => p.Id, id);
			var update = Builders<T>.Update.Set(field, value);
			await Collection.UpdateOneAsync(filter, update);
		}

		public virtual async void AddDocument(T document) 
			=> await Collection.InsertOneAsync(document);
	}
}
