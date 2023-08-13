using MongoDB.Bson;

namespace deli.data.MongoDB.Types
{
	public struct BsonObjectId
	{
		public const string Empty = "000000000000000000000000";
		public string StringId { get; } = Empty;

		public BsonObjectId()
			=> StringId = Empty;

		public BsonObjectId(string value)
			=> StringId = value;

		public static BsonObjectId Create(string value)
			=> new BsonObjectId(value);

		public static implicit operator BsonObjectId(string bsonObjectId)
			=> Create(bsonObjectId);

		public static implicit operator string(BsonObjectId bsonObjectId)
			=> bsonObjectId.StringId;

		public static implicit operator BsonObjectId(ObjectId objectId)
			=> new BsonObjectId(objectId.ToString());

		public static implicit operator ObjectId(BsonObjectId bsonObjectId)
			=> ObjectId.Parse(bsonObjectId.StringId);

		public static BsonObjectId[] ConvertStrArray(string[] values)
			=> Array.ConvertAll(values, x => new BsonObjectId(x));
	}
}
