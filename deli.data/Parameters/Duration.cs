namespace deli.data.Parameters
{
	public class Duration : Parameter<Duration>
	{
		public const double MIN_DURATION = 1;
		public const double MAX_DURATION = 8765.81277; // 1 год
		public double MinDuration { get; set; } = MIN_DURATION;
		public double MaxDuration { get; set; } = MAX_DURATION;
	}
}
