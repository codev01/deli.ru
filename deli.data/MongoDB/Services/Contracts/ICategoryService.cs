namespace deli.data.MongoDB.Services.Contracts
{
	public interface ICategoryService : IMongo_Service_Base<Category>
	{
		Task<IEnumerable<Category>> GetCategories();
	}
}
