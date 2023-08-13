namespace deli.data.MongoDB.Common
{
	public class CollectionDependency
	{
		public string CollectionName { get; set; }
		public MongoManager MongoManager { get; set; }

		public CollectionDependency(string collectionName, MongoManager mongoManager)
		{
			CollectionName = collectionName;
			MongoManager = mongoManager;
		}
	}
}
