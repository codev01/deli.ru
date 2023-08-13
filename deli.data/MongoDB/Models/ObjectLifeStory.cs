namespace deli.data.Models
{
	/// <summary>
	/// История жизни объекта на сервисе
	/// </summary>
	public class ObjectLifeStory
	{
		public double Rating { get; set; }
		public BsonObjectId[] Feedbacks { get; set; }
		public BsonObjectId[] Transactions { get; set; }
	}
}
