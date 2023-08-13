namespace deli.data.Models.Contracts
{
	public interface IDataBaseConnection
	{
		string DataBaseName { get; }
		string ConnectionString { get; }
	}
}
