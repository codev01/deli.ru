namespace data.deli.ru.MongoDB.Services.Contracts
{
	public interface IProductService : IMongoServiceBase<Product>
	{
		Task<IEnumerable<Product>> GetProducts(string searchString, 
											   string categoryId,
											   double minRentPrice = DefaultParams.MIN_PRICE,
											   double maxRentPrice = DefaultParams.MAX_PRICE,
											   double minFullPrice = DefaultParams.MIN_PRICE,
											   double maxFullPrice = DefaultParams.MAX_PRICE,
											   double startLatitude = Location.MIN_LATITUBE,
											   double startLongitude = Location.MIN_LONGITUBE,
											   double endLatitude = Location.MAX_LATITUBE,
											   double endLongitude = Location.MAX_LONGITUBE,
											   uint limit = DefaultParams.LARGE_LIMIT,
											   uint offset = DefaultParams.OFFSET_DEFAULT);
	}
}
