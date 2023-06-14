namespace data.deli.ru.Models
{
	public class UserPrivateInfo
	{
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public Location[] Locations { get; set; }
	}
}
