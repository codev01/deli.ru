namespace data.deli.ru.Models
{
    public class Feedback : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public double Rating { get; set; }
		public BsonObjectId FromId { get; set; }
		public BsonObjectId ToId { get; set; }
		public string Message { get; set; }
	}
}
