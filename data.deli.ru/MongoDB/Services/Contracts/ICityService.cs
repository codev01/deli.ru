using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using data.deli.ru.MongoDB.Models;

namespace data.deli.ru.MongoDB.Services.Contracts
{
	public interface ICityService
	{
		Task<IEnumerable<City>> SearchCities(string searchString, int limit = 100, int offset = 0);
	}
}
