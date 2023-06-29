using System.Text.Json;
using System.Text.Json.Serialization;

namespace data.deli.ru.MongoDB.Serializers
{
	public class JsonObjectIdArraySerializer : JsonConverter<BsonObjectId[]>
	{
		public override BsonObjectId[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}

		public override void Write(Utf8JsonWriter writer, BsonObjectId[] value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (var item in value)
			{
				writer.WriteStringValue(item.StringId);
			}
			writer.WriteEndArray();
		}
	}
}
