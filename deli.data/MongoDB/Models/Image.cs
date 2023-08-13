using MongoDB.Bson.Serialization.Attributes;

namespace deli.data.Models
{
	public class Image
	{
		[BsonElement("_id")]
		public BsonObjectId Id { get; set; }
		public string Uri { get; set; }
	}
}