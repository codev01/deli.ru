using data.deli.ru.Bases;
using data.deli.ru.MongoDB.Services.Contracts;

using MongoDB.Driver;

namespace data.deli.ru.MongoDB.Services
{
	public class CategoryService : MongoServiceBase<Category>, ICategoryService
	{
		public CategoryService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main, "categories") { }

		public async Task<IEnumerable<Category>> GetCategories()
			=> await Collection.Find(_ => true).ToListAsync();
	}
}
