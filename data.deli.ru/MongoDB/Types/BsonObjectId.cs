using MongoDB.Bson;

namespace data.deli.ru.MongoDB.Types
{
	public struct BsonObjectId
	{
		public const string Empty = "000000000000000000000000";

		public string Value { get; }

		public BsonObjectId()
			=> Value = Empty;

		public BsonObjectId(string value)
			=> Value = value;

		public static BsonObjectId Create(string value)
			=> new BsonObjectId(value);

		public static implicit operator BsonObjectId(ObjectId objectId)
			=> new BsonObjectId(objectId.ToString());

		public static implicit operator ObjectId(BsonObjectId myObjectId)
			=> ObjectId.Parse(myObjectId.Value);

		public static BsonObjectId[] ConvertStrArray(string[] values)
			=> Array.ConvertAll(values, x => new BsonObjectId(x));
	}
}
