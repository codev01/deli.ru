namespace data.deli.ru.Models
{
	public enum TransactionStatus
	{
		Open,
		Completed,
		Closed
	}

	public class Transaction : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public BsonObjectId ProductId { get; set; }
		public BsonObjectId TenantId { get; set; }
		public BsonObjectId LandlordId { get; set; }
		public int Duration { get; set; }
		public TransactionStatus Status { get; set; }
	}
}
