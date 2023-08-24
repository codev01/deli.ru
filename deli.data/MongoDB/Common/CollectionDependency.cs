namespace deli.data.MongoDB.Common
{
	public class CollectionDependency
	{
		public string CollectionName { get; set; }
		public _MongoManager MongoManager { get; set; }

		public CollectionDependency(string collectionName, _MongoManager mongoManager)
		{
			CollectionName = collectionName;
			MongoManager = mongoManager;
		}
	}
}
