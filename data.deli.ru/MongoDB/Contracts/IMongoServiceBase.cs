using System.Linq.Expressions;

using MongoDB.Driver;

namespace data.deli.ru.MongoDB.Contracts
{
	public interface IMongoServiceBase<T> where T : IBsonObject
	{
		Task<IEnumerable<T>> GetById(params string[] ids);
		void UdateField<TField>(BsonObjectId id, Expression<Func<T, TField>> field, TField value);
		void AddDocument(T document);
	}
}
