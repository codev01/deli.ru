using deli.data.Bases;
using deli.data.MongoDB.Services.Contracts;

using MongoDB.Driver;

namespace deli.data.MongoDB.Services
{
	public class CategoryService : MongoServiceBase<Category>, ICategoryService
	{
		public CategoryService(DataBaseManager dataBaseManager) : base(dataBaseManager.Common.Categories) { }

		public async Task<IEnumerable<Category>> GetCategories() => await Collection
			.Find(_ => true)
			.ToListAsync();
	}
}
