﻿using data.deli.ru.Bases;
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


		public async Task<IEnumerable<Product>> GetProducts(BsonObjectId announcementId, 
															PriceMaxMin price, 
															Durations durations, 
															Constraint constraint)
		{
			MongoExpressionManager expressions = new MongoExpressionManager();

			var builder = Builders<Product>.Filter;
			var filters = new List<FilterDefinition<Product>>();

			#region Price
			if (price.MinRentPrice > price.MaxRentPrice && price.MinFullPrice > price.MaxFullPrice)
				throw new ArgumentException("min...Price must not be greater than max...Price");

			filters.Add(builder.Gte(p => p.RentPrice, price.MinRentPrice) &
						builder.Lte(p => p.RentPrice, price.MaxRentPrice));

			filters.Add(builder.Gte(p => p.FullPrice, price.MinFullPrice) &
						builder.Lte(p => p.FullPrice, price.MaxFullPrice));
			#endregion


			if(durations.Hours != Durations.DefaultHours)
				filters.Add(builder.All(p => p.Durations.Hours, durations.Hours));

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
