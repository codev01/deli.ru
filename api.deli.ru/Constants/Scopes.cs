namespace api.deli.ru.Constants
{
	public class Scopes
	{
		public const string All = $"{Categories}, {Products}, {Cities}, {Transactions}, {Feedbacks}, {Users}, {Announcements}, {ConfidentionalInfo}, {Serach}";
		public const string Categories = "categories";
		public const string Products = "products";
		public const string Cities = "cities";
		public const string Transactions = "transactions";
		public const string Feedbacks = "feedbacks";
		public const string Users = "users";
		public const string Announcements = "announcements";
		public const string ConfidentionalInfo = "confidentional_info";
		public const string Serach = "search";
		public const string Auth = "auth";
	}
}
