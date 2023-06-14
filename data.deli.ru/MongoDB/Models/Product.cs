using MongoDB.Bson;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.Models
{
    public class Product : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public DisplayInfo DisplayInfo { get; set; }
		public Location Location { get; set; }
		public double RentPrice { get; set; }
		public double FullPrice { get; set; }
		public int Count { get; set; }
		public BsonObjectId LandlordId { get; set; }
		public BsonObjectId CategoryId { get; set; }
		public Image[] Images { get; set; }
		public int[] Durations { get; set; }
		public ObjectLifeStory LifeStory { get; set; }
	}
}
