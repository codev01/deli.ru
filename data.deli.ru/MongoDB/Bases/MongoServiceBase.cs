using data.deli.ru.MongoDB.Bases;

using MongoDB.Driver;

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
	}
}
