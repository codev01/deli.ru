using deli.data.MongoDB.Models;

namespace deli.data.MongoDB.Services.Contracts
{
	public interface IAnnouncementService : IMongoServiceBase<Announcement>
	{
		Task AddAnnouncement(Announcement announcement);
		Task<IEnumerable<Announcement>> GetAnnouncements(string searchString,
														 string categoryId,
														 bool? isPublished,
														 Radius radius,
														 Constraint constraint);
	}
}
