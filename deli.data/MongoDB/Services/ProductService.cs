using deli.data.Bases;
using deli.data.MongoDB.Extensions;
using deli.data.MongoDB.Services.Contracts;

using MongoDB.Driver;

using BsonObjectId = deli.data.MongoDB.Types.BsonObjectId;

namespace deli.data.MongoDB.Services
{
	public class ProductService : MongoServiceBase<Product>, IProductService
	{
		private static readonly IAnnouncementService _announcementService;

		static ProductService()
			=> _announcementService = GetService<IAnnouncementService>();

		public ProductService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main.Products) { }

		public async Task AddProduct(string announcementId, params Product[] products)
		{
			_announcementService.UpdateField(BsonObjectId.Create(announcementId), a => a.Count, products.Length);
			foreach (var product in products)
				product.AnnouncementId = announcementId;
			await Collection.InsertManyAsync(products);
		}

		public async Task<IEnumerable<Product>> GetProducts(string announcementId,
															PriceMaxMin price,
															Duration duration,
															Constraint constraint)
		{
			MongoExpressionManager expressions = new MongoExpressionManager();

			var builder = Builders<Product>.Filter;
			var filters = new List<FilterDefinition<Product>>();

			filters.Add(builder.Eq(p => p.AnnouncementId, BsonObjectId.Create(announcementId)));

			#region Price
			if (price.MinRentPrice > price.MaxRentPrice && price.MinFullPrice > price.MaxFullPrice)
				throw new ArgumentException("min...Price must not be greater than max...Price");

			filters.Add(builder.Gte(p => p.RentPrice, price.MinRentPrice) &
						builder.Lte(p => p.RentPrice, price.MaxRentPrice));

			filters.Add(builder.Gte(p => p.FullPrice, price.MinFullPrice) &
						builder.Lte(p => p.FullPrice, price.MaxFullPrice));
			#endregion

			filters.Add(builder.Gte(p => p.Durations.MinDuration, duration.MinDuration) &
						builder.Lte(p => p.Durations.MaxDuration, duration.MaxDuration));

			//var projection = Builders<Product>.Projection.Include(p => p.RentPrice)
			//										       .Include(p => p.Count);

			var result = Collection
				.Find(builder.Combine(filters))
				/*.Project<Product>(projection)*/
				.Skip((int)constraint.Offset)
				.Limit((int)constraint.Limit)
				.ToListAsync();
			return await result;
		}
	}
}
