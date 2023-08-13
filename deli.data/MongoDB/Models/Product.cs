﻿using MongoDB.Bson.Serialization.Attributes;

using BsonObjectId = deli.data.MongoDB.Types.BsonObjectId;

namespace deli.data.Models
{
	public class Product : IBsonObject, IDateObject
	{
		public BsonObjectId Id { get; set; }
		public BsonObjectId AnnouncementId { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModify { get; set; }
		public string Description { get; set; }
		public double RentPrice { get; set; }
		public double FullPrice { get; set; }
		public int Count { get; set; }
		[BsonElement("durations")]
		public Durations Durations { get; set; }
		public Image[] Images { get; set; }
	}
}