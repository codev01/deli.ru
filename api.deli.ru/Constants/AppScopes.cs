namespace api.deli.ru.Constants
{
    public class AppScopes
    {
        public const string All = $"{Categories}, {Products}, {Cities}, {Transactions}, {Feedbacks}, {Users}, {Announcements}, {ConfidentionalInfo}";
        public const string Categories = "categories";
        public const string Products = "products";
        public const string Cities = "cities";
        public const string Transactions = "transactions";
        public const string Feedbacks = "feedbacks";
        public const string Users = "users";
        public const string Announcements = "fnnouncements";
        public const string ConfidentionalInfo = "confidentional_info";
    }
}
