namespace data.deli.ru.Models
{
	public enum CategoryType
	{
		Low,
		High
	}

	public class Category : IBsonObject
	{
		public BsonObjectId Id { get; set; }
		public string Name { get; set; }
		public Image Image { get; set; }
		public BsonObjectId ParentId { get; set; }
	}
}