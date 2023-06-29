using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.MongoDB.Serializers
{
	public class BsonObjectIdArraySerializer : SerializerBase<BsonObjectId[]>
	{
		public override BsonObjectId[] Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			BsonArray bsonArray = BsonSerializer.Deserialize<BsonArray>(context.Reader);
			string[] objectIds = bsonArray.Select(bson => bson.AsObjectId.ToString()).ToArray();
			return objectIds.Select(id => new BsonObjectId(id)).ToArray();
		}

		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BsonObjectId[] value)
		{
			context.Writer.WriteStartArray();
			foreach (var bsonObjectId in value)
			{
				context.Writer.WriteObjectId(new ObjectId(bsonObjectId.StringId));
			}
			context.Writer.WriteEndArray();
		}
	}
}
