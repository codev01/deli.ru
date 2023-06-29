using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.MongoDB.Serializers
{
	public class BsonObjectIdSerializer : SerializerBase<BsonObjectId>
	{
		public override BsonObjectId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
			=> new BsonObjectId(context.Reader.ReadObjectId().ToString());

		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BsonObjectId value)
			=> context.Writer.WriteObjectId(ObjectId.Parse(value.StringId));
	}
}
