namespace deli.data.Parameters
{
	public class Location
	{
		public const double MIN_LATITUDE = -90;
		public const double MAX_LATITUDE = 90;
		public const double MIN_LONGITUDE = -180;
		public const double MAX_LONGITUDE = 180;

		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Location(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		public static Location MinLocation()
			=> new Location(MIN_LATITUDE, MIN_LONGITUDE);
		public static Location MaxLocation()
			=> new Location(MAX_LATITUDE, MAX_LONGITUDE);
	}
}
