using MongoDB.Bson.Serialization.Attributes;

namespace data.deli.ru.Models
{
	public class Image
	{
		[BsonElement("_id")]
		public BsonObjectId Id { get; set; }
		public string Uri { get; set; }
	}
}