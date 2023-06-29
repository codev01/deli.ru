using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace data.deli.ru.MongoDB.Serializers
{
	public class JsonObjectIdSerializer : JsonConverter<BsonObjectId>
	{
		public override BsonObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}

		public override void Write(Utf8JsonWriter writer, BsonObjectId value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.StringId);
		}
	}
}
