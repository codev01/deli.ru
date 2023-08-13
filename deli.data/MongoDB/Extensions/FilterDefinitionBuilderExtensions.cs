using MongoDB.Driver;

namespace deli.data.MongoDB.Extensions
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
