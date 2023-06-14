using Microsoft.AspNetCore.Mvc;

namespace data.deli.ru.Types
{
	public class Location
	{
        public const double MIN_LATITUBE = -90;
		public const double MAX_LATITUBE = 90;
		public const double MIN_LONGITUBE = -180;
		public const double MAX_LONGITUBE = 180; 
		//[FromQuery]
		public double Latitude { get; set; }
		//[FromQuery]
		public double Longitude { get; set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

		public static Location MinLocation() 
			=> new Location(MIN_LATITUBE, MIN_LONGITUBE);
		public static Location MaxLocation()
			=> new Location(MAX_LATITUBE, MAX_LONGITUBE);
	}
}
