using deli.data.MongoDB.Models;

namespace deli.data.MongoDB.Services.Contracts
{
	public interface ICityService
	{
		Task<IEnumerable<City>> SearchCities(string searchString, int limit = 100, int offset = 0);
	}
}
