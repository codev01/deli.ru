using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.Models
{
	public class Product : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public BsonObjectId AnnouncementId { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public string Description { get; set; }
		public double RentPrice { get; set; }
		public double FullPrice { get; set; }
		public int Count { get; set; }
		public Durations Durations { get; set; }
		public Image[] Images { get; set; }
	}
}
