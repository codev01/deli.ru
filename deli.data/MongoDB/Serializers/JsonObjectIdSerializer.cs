using System.Text.Json;
using System.Text.Json.Serialization;

namespace deli.data.MongoDB.Serializers
{
	public class JsonObjectIdSerializer : JsonConverter<BsonObjectId>
	{
		public override BsonObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return BsonObjectId.Create(reader.GetString());
		}

		public override void Write(Utf8JsonWriter writer, BsonObjectId value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.StringId);
		}
	}
}
