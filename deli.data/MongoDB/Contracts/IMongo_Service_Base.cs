using System.Linq.Expressions;

using MongoDB.Driver;

namespace deli.data.MongoDB.Contracts
{
	public interface IMongo_Service_Base<T> where T : IBsonObject
	{
		Task<IEnumerable<T>> GetById(params string[] ids);
		void UpdateField<TField>(BsonObjectId id, Expression<Func<T, TField>> field, TField value);
		void AddDocument(T document);
		void AddItemArray<TField>(BsonObjectId id, Expression<Func<T, IEnumerable<TField>>> field, TField value);
		Task<DeleteResult> DeleteById(string[] ids);
	}
}
