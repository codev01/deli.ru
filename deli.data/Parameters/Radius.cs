namespace deli.data.Parameters
{
	public class Radius
	{
		public double StartLatitude { get; set; } = Location.MIN_LATITUDE;
		public double EndLatitude { get; set; } = Location.MAX_LATITUDE;
		public double StartLongitude { get; set; } = Location.MIN_LONGITUDE;
		public double EndLongitude { get; set; } = Location.MAX_LONGITUDE;
	}
}
