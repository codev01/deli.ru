namespace data.deli.ru.MongoDB.Services.Contracts
{
	public interface ICategoryService : IMongoServiceBase<Category>
	{
		Task<IEnumerable<Category>> GetCategories();
	}
}
