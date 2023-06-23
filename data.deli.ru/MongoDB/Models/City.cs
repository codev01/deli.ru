namespace data.deli.ru.MongoDB.Models
{
	public class City : IBsonObject
	{
		public BsonObjectId Id { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
	}
}
