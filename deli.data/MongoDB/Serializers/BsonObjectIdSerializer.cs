using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using BsonObjectId = deli.data.MongoDB.Types.BsonObjectId;

namespace deli.data.MongoDB.Serializers
{
	public class BsonObjectIdSerializer : SerializerBase<BsonObjectId>
	{
		public override BsonObjectId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
			=> new BsonObjectId(context.Reader.ReadObjectId().ToString());

		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BsonObjectId value)
			=> context.Writer.WriteObjectId(ObjectId.Parse(value.StringId ?? BsonObjectId.Empty));
	}
}
