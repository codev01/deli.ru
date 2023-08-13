using deli.data.Bases;
using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;

using MongoDB.Bson;

using MongoDB.Driver;

namespace deli.data.MongoDB.Services
{
	public class CityService : MongoServiceBase<City>, ICityService
	{
		public CityService(DataBaseManager dataBaseManager) : base(dataBaseManager.Common.Cities) { }

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
