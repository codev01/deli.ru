namespace data.deli.ru.MongoDB.Contracts
{
	public interface IMongoServiceBase<T> where T : IBsonObject
	{
		Task<IEnumerable<T>> GetById(string[] ids);
	}
}
