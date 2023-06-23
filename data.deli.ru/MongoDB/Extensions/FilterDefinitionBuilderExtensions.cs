using MongoDB.Driver;

namespace data.deli.ru.MongoDB.Extensions
{
	internal static class FilterDefinitionBuilderExtensions
	{
		public static FilterDefinition<TDocument> Combine<TDocument>(this FilterDefinitionBuilder<TDocument> builder, IEnumerable<FilterDefinition<TDocument>> filters)
		{
			var filter = Builders<TDocument>.Filter.Empty;
			foreach (var f in filters)
				filter &= f;
			return filter;
		}
	}
}
