namespace data.deli.ru.MongoDB.Models
{
	public class App : IBsonObject
	{
		public BsonObjectId Id { get; set; }
		public string Name { get; set; }
		public string ClientSecret { get; set; }
		public double Version { get; set; }
		public string[] Scopes { get; set; }

		public App(string app_id, string client_secret)
		{
			Id = BsonObjectId.Create(app_id);
			ClientSecret = client_secret;
		}
	}
}
