using System.ComponentModel.DataAnnotations;

using data.deli.ru.Bases;
using data.deli.ru.Common;
using data.deli.ru.MongoDB.Extensions;
using data.deli.ru.MongoDB.Services.Contracts;

using MongoDB.Bson;
using MongoDB.Driver;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.MongoDB.Services
{
    public class ProductService : MongoServiceBase<Product>, IProductService
	{
		public ProductService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main, "products") { }

		public async Task<IEnumerable<Product>> GetProducts(string searchString,
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
															uint offset = DefaultParams.OFFSET_DEFAULT){
			MongoExpressionManager expressions = new MongoExpressionManager();



			var builder = Builders<Product>.Filter;
			var filters = new List<FilterDefinition<Product>>();

			#region SearchString
			if (!string.IsNullOrEmpty(searchString))
			{
				filters.Add(builder.Regex(p => p.DisplayInfo.Name, new BsonRegularExpression($".*{searchString}.*", "i")));

				//expressions.Add(new MongoExpression("displayInfo.name", 
				//				MongoParameter.Regex($".*{searchString}.*"),
				//				MongoParameter.Options( "i")
				//));
			}
			else
				throw new ArgumentException("searchString = null or empty");
			#endregion

			#region CategoryId
			if (!string.IsNullOrEmpty(categoryId) && categoryId != BsonObjectId.Empty)
				filters.Add(builder.Eq(p => p.CategoryId.Value, categoryId));
			#endregion

			//expressions.Add(new MongoExpression("categoryId", MongoParameter.ObjectId(categoryId)));

			//expressions.Add(new MongoExpression("rentPrice",
			//	 MongoParameter.Gte(minRentPrice),
			//	 MongoParameter.Lte(maxRentPrice)
			//));

			#region Price
			if (minRentPrice > maxRentPrice)
				throw new ArgumentException("minRentPrice must not be greater than maxRentPrice");

			filters.Add(builder.Gte(p => p.RentPrice, minRentPrice) & 
						builder.Lte(p => p.RentPrice, maxRentPrice));

			filters.Add(builder.Gte(p => p.FullPrice, minFullPrice) &
						builder.Lte(p => p.FullPrice, maxFullPrice));
			#endregion

			#region Location
			var v = startLatitude;
			if (startLatitude > endLatitude)
			{
				v = startLatitude;
				startLatitude = endLatitude;
				endLatitude = v;
			}
			if (startLongitude > endLongitude)
			{
				v = startLongitude;
				startLongitude = endLongitude;
				endLongitude = v;
			}
			filters.Add(builder.Gte(p => p.Location.Latitude, startLatitude));
			filters.Add(builder.Lte(p => p.Location.Latitude, endLatitude));
			filters.Add(builder.Gte(p => p.Location.Longitude, startLongitude));
			filters.Add(builder.Lte(p => p.Location.Longitude, endLongitude));
			#endregion

			//expressions.Add(new MongoExpression("location.latitude",
			//		MongoParameter.Gte(startLatitube),
			//		MongoParameter.Lte(endLatitube)
			//	),
			//	new MongoExpression("location.longitude", 
			//		MongoParameter.Gte(startLongitude),
			//		MongoParameter.Lte(endLongitude)
			//	)
			//);

			var projection = Builders<Product>.Projection.Include(p => p.DisplayInfo.Name)
																		   .Include(p => p.Location)
																		   .Include(p => p.RentPrice)
																		   .Include(p => p.Count);


			//string expression = BsonDocument.Parse(expressions.CreateExpressionString()).ToString();


			var result = Collection.Find(builder.Combine(filters)).Project<Product>(projection).Skip((int)offset).Limit((int)limit).ToListAsync();

			return await result;
		}
	}
}
