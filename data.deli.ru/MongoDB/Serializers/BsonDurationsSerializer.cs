using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace data.deli.ru.MongoDB.Serializers
{
	public class BsonDurationsSerializer : SerializerBase<Durations>
	{
		public override Durations Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			Durations bsonArray = BsonSerializer.Deserialize<Durations>(context.Reader);
			//double[] doubleArray = bsonArray.Select(bson => bson.AsDouble).ToArray();
			return bsonArray; //new Durations(doubleArray);
		}

		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Durations value)
		{
			context.Writer.WriteStartDocument();

			context.Writer.WriteStartArray(nameof(Durations.Hours));
			foreach (var item in value.Hours)
				context.Writer.WriteDouble(item);
			context.Writer.WriteEndArray();

			context.Writer.WriteEndDocument();
		}
	}
}
