﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.deli.ru.MongoDB.Models
{
	public class Account : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public bool IsLogined { get; set; }
		public string Password { get; set; }
		public string OldPassword { get; set; }
		public string Role { get; set; }
		public string Email { get; set; }
		public string MobilePhone { get; set; }

		public Account(string account_id, string password)
		{
			Id = BsonObjectId.Create(account_id);
			Password = password;
		}
	}
}