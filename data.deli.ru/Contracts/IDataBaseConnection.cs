namespace data.deli.ru.Contracts
{
	public interface IDataBaseConnection
	{
		string DataBaseName { get; }
		string ConnectionString { get; }
	}
}
