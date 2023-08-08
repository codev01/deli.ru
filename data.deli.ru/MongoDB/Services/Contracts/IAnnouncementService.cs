using data.deli.ru.MongoDB.Models;
using data.deli.ru.Parameters;

namespace data.deli.ru.MongoDB.Services.Contracts
{
    public interface IAnnouncementService : IMongoServiceBase<Announcement>
	{
		Task<IEnumerable<Announcement>> GetAnnouncements(string searchString,
														 string categoryId,
														 bool? isPublished,
														 Radius radius,
														 Constraint constraint);
	}
}
