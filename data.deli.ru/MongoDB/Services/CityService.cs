using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using data.deli.ru.Bases;
using data.deli.ru.Models;
using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;
using MongoDB.Bson;

using MongoDB.Driver;

namespace data.deli.ru.MongoDB.Services
{
	public class CityService : MongoServiceBase<City>, ICityService
	{
		public CityService(DataBaseManager dataBaseManager) : base(dataBaseManager.Common, "cities") { }

		public async Task<IEnumerable<City>> SearchCities(string searchString, int limit = 100, int offset = 0)
		{
			MongoExpressionManager expressions = new MongoExpressionManager();
			MongoParameter[] parameters =
			{
				MongoParameter.Regex($".*{searchString}.*"),
				MongoParameter.Options( "i")
			};

			if (!string.IsNullOrEmpty(searchString))
			{
				expressions.Or(
					new MongoExpression("region", parameters),
					new MongoExpression("name", parameters)
				);
			}

			string expression = BsonDocument.Parse(expressions.CreateExpressionString()).ToString();
			var result = Collection.Find(expression).Skip(offset).Limit(limit).ToListAsync();

			return await result;
		}
	}
}
