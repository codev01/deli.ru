using data.deli.ru.Parameters;

namespace data.deli.ru.MongoDB.Services.Contracts
{
    public interface IProductService : IMongoServiceBase<Product>
	{
		public Task<IEnumerable<Product>> GetProducts(BsonObjectId announcementId,
													  PriceMaxMin price,
													  Durations durations,
													  Constraint constraint);
	}
}
