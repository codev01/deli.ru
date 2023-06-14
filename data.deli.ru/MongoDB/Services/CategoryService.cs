using System.Reflection.PortableExecutable;

using data.deli.ru.Bases;
using data.deli.ru.MongoDB;
using data.deli.ru.MongoDB.Services.Contracts;

using MongoDB.Bson;
using MongoDB.Driver;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.MongoDB.Services
{
    public class CategoryService : MongoServiceBase<Category>, ICategoryService
	{
		public CategoryService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main, "categories") { }

		public async Task<IEnumerable<Category>> GetCategories() 
			=> await Collection.Find(_ => true).ToListAsync();
	}
}
