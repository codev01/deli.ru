using deli.runtime.Bases;
using deli.runtime.Containers;

namespace deli.runtime
{
	public static class _ContainerManager
	{
		private static Dictionary<string, ApplicationContainer_> Applications { get; set; } = new();
		private static Dictionary<string, UserContainer_> Users { get; set; } = new();

		public static ApplicationContainer_? GetApp(string appId)
			=> Applications.GetValueOrDefault(appId);
		public static void AddApp(ApplicationContainer_ appContainer)
		{
			Applications.Remove(appContainer.Key);
			Applications.Add(appContainer.Key, appContainer);
			appContainer.OnTerminated += ContainerTerminated;
		}
		public static void RemoveApp(string key)
			=> Applications.Remove(key);

		public static UserContainer_? GetUser(string userId)
			=> Users.GetValueOrDefault(userId);
		public static void AddUser(UserContainer_ userContainer)
		{
			Users.Remove(userContainer.Key);
			Users.Add(userContainer.Key, userContainer);
			userContainer.OnTerminated += ContainerTerminated;
		}
		public static void RemoveUser(string key)
			=> Applications.Remove(key);

		private static void ContainerTerminated(Container_Base container)
		{
			container.OnTerminated -= ContainerTerminated;
			container.Dispose();
			Users.Remove(container.Key);
		}
	}
}