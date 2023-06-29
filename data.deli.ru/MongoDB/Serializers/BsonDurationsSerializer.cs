using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			double[] doubleArray = null;

			context.Reader.ReadStartDocument();
			while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
			{
				string fieldName = context.Reader.ReadName();

				if (fieldName == nameof(Durations.Hours))
				{
					context.Reader.ReadStartArray();
					var doubleList = new List<double>();
					while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
						doubleList.Add(context.Reader.ReadDouble());
					context.Reader.ReadEndArray();
					doubleArray = doubleList.ToArray();
				}
			}
			context.Reader.ReadEndDocument();

			return new Durations(doubleArray);
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
