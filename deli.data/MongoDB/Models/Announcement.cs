namespace deli.data.MongoDB.Models
{
	public class Announcement : IBsonObject
	{
		public BsonObjectId Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public BsonObjectId LandlordId { get; set; }
		public BsonObjectId CategoryId { get; set; }
		public bool IsPublished { get; set; }
		public int Count { get; set; }
		public string Name { get; set; }
		public Location Location { get; set; }
		public ObjectLifeStory LifeStory { get; set; }
		public Product PreviewProduct { get; set; }
	}
}
