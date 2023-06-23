namespace api.deli.ru.ConfigModels
{
	public class ApiConfigurations
	{
		public DatabaseConnections DatabaseConnections { get; set; }
		public Swagger Swagger { get; set; }
		public string LocalFilesPath { get; set; }
		public string[] UseUrls { get; set; }
	}
}
