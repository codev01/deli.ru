using data.deli.ru.MongoDB.Models;

namespace data.deli.ru.MongoDB.Services.Contracts
{
	public interface ICityService
	{
		Task<IEnumerable<City>> SearchCities(string searchString, int limit = 100, int offset = 0);
	}
}
