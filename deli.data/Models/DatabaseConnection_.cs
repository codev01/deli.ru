using deli.data.Models.Contracts;

namespace deli.data.Models
{
	public class DatabaseConnection_ : IDataBaseConnection_
	{
		public string DataBaseName { get; set; }
		public string ConnectionString { get; set; }
	}
}
