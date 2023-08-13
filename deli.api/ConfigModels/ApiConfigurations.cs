namespace deli.api.ConfigModels
{
	public class ApiConfigurations
	{
		public DatabaseConnections DatabaseConnections { get; set; }
		public Swagger Swagger { get; set; }
		public EndPoints EndPoints { get; set; }
		public Settings Settings { get; set; }
		public string LocalFilesPath { get; set; }
	}
}
