using deli.data.Bases;
using deli.data.MongoDB.Extensions;
using deli.data.MongoDB.Models;
using deli.data.MongoDB.Services.Contracts;


using MongoDB.Bson;
using MongoDB.Driver;

using BsonObjectId = deli.data.MongoDB.Types.BsonObjectId;

namespace deli.data.MongoDB.Services
{
	public class AnnouncementService : MongoServiceBase<Announcement>, IAnnouncementService
	{
		private static IProductService _productService;

		static AnnouncementService()
		{
			_productService = GetService<IProductService>();
		}

		public AnnouncementService(DataBaseManager dataBaseManager) : base(dataBaseManager.Main.Announcements) { }

		public async Task AddAnnouncement(Announcement announcement)
		{
			await Collection.InsertOneAsync(announcement);
		}

		public async Task<IEnumerable<Announcement>> GetAnnouncements(string searchString,
																	  string categoryId,
																	  bool? isPublished,
																	  Radius radius,
																	  Constraint constraint)
		{
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

			if (isPublished is not null)
				filters.Add(builder.Eq(a => a.IsPublished, isPublished));

			//var projection = Builders<Announcement>.Projection
			//	.Include(a => a.Id)
			//	.Include(a => a.Name)
			//	.Include(a => a.Location);

			var result = await Collection
				.Find(builder.Combine(filters))
				//.Project<Announcement>(projection)
				.Skip((int)constraint.Offset)
				.Limit((int)constraint.Limit)
				.ToListAsync();

			foreach (Announcement announcement in result)
			{
				var products = await _productService.GetProducts(announcement.Id, PriceMaxMin.Default, Duration.Default, constraint);
				announcement.PreviewProduct = products?.FirstOrDefault();
			}

			return result;
		}
	}
}
