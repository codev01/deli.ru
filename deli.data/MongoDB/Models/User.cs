namespace deli.data.Models
{
	public class User : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Image Avatar { get; set; }
		public bool IsLandlord { get; set; }
		public BsonObjectId[] Favorites { get; set; }
		public ObjectLifeStory Tenant { get; set; }
		public ObjectLifeStory Landlord { get; set; }
		public UserPrivateInfo confidentialInfo { get; set; }
	}
}
