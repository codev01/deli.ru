using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.deli.ru.MongoDB.Models
{
	public class App : IBsonObject
	{
		public BsonObjectId Id { get; set; }
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
