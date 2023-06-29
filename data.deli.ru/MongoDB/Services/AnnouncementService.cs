using data.deli.ru.Bases;
using data.deli.ru.MongoDB.Extensions;
using data.deli.ru.MongoDB.Models;
using data.deli.ru.MongoDB.Services.Contracts;
using data.deli.ru.Parameters;
using MongoDB.Bson;
using MongoDB.Driver;

using BsonObjectId = data.deli.ru.MongoDB.Types.BsonObjectId;

namespace data.deli.ru.MongoDB.Services
{
    public class AnnouncementService : MongoServiceBase<Announcement>, IAnnouncementService
	{
		public AnnouncementService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main, "announcements") { }

		public async Task<IEnumerable<Announcement>> GetPublishedAnnouncements(string searchString,
																			   string categoryId,
																			   Radius radius,
																			   Constraint constraint){
			var builder = Builders<Announcement>.Filter;
			var filters = new List<FilterDefinition<Announcement>>();

			#region SearchString
			if (!string.IsNullOrEmpty(searchString))
				filters.Add(builder.Regex(a => a.Name, new BsonRegularExpression($".*{searchString}.*", "i")));
			else
				throw new ArgumentException("searchString = null or empty");
			#endregion

			#region CategoryId
			if (!string.IsNullOrEmpty(categoryId) && categoryId != BsonObjectId.Empty)
				filters.Add(builder.Eq(a => a.CategoryId.StringId, categoryId));
			#endregion

			#region Location
			var v = radius.StartLatitude;
			if (radius.StartLatitude > radius.EndLatitude)
			{
				v = radius.StartLatitude;
				radius.StartLatitude = radius.EndLatitude;
				radius.EndLatitude = v;
			}
			if (radius.StartLongitude > radius.EndLongitude)
			{
				v = radius.StartLongitude;
				radius.StartLongitude = radius.EndLongitude;
				radius.EndLongitude = v;
			}
			filters.Add(builder.Gte(p => p.Location.Latitude, radius.StartLatitude));
			filters.Add(builder.Lte(p => p.Location.Latitude, radius.EndLatitude));
			filters.Add(builder.Gte(p => p.Location.Longitude, radius.StartLongitude));
			filters.Add(builder.Lte(p => p.Location.Longitude, radius.EndLongitude));
			#endregion

			filters.Add(builder.Eq(a => a.IsPublished, true));

			//var projection = Builders<Announcement>.Projection
			//	.Include(a => a.Id)
			//	.Include(a => a.Name)
			//	.Include(a => a.Location);

			var result = Collection
				.Find(builder.Combine(filters))
				//.Project<Announcement>(projection)
				.Skip((int)constraint.Offset)
				.Limit((int)constraint.Limit)
				.ToListAsync();
			return await result;
		}
	}
}
