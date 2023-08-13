using System.Text.Json;
using System.Text.Json.Serialization;

namespace deli.data.MongoDB.Serializers
{
	public class JsonDurationsSerializer : JsonConverter<Durations>
	{
		public override Durations Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using var doc = JsonDocument.ParseValue(ref reader);
			var doubleArray = doc.RootElement.GetProperty(nameof(Durations.Hours)).EnumerateArray().Select(x => x.GetDouble()).ToArray();

			return new Durations(doubleArray);
		}

		public override void Write(Utf8JsonWriter writer, Durations value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("hours");
			writer.WriteStartArray();
			foreach (var item in value.Hours)
				writer.WriteNumberValue(item);
			writer.WriteEndArray();
			writer.WriteEndObject();
		}
	}
}
