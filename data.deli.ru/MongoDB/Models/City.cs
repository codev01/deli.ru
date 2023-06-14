using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.deli.ru.MongoDB.Models
{
	public class City : IBsonObject
	{
		public BsonObjectId Id { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
	}
}
