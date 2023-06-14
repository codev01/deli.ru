using data.deli.ru.Contracts;

namespace data.deli.ru.Models
{
	public class DatabaseConnection : IDataBaseConnection
	{
		public string DataBaseName { get; set; }
		public string ConnectionString { get; set; }
	}
}
