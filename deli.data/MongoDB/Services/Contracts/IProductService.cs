namespace deli.data.MongoDB.Services.Contracts
{
	public interface IProductService : IMongo_Service_Base<Product>
	{
		Task AddProduct(string announcementId, params Product[] products);
		Task<IEnumerable<Product>> GetProducts(string announcementId,
													  PriceMaxMin price,
													  Duration duration,
													  Constraint constraint);
	}
}
