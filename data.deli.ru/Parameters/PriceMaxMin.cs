namespace data.deli.ru.Parameters
{
	public class PriceMaxMin
	{
		public const double MIN_PRICE = 0;
		public const double MAX_PRICE = 1000000;

		public double MinRentPrice { get; set; } = MIN_PRICE;
		public double MaxRentPrice { get; set; } = MAX_PRICE;
		public double MinFullPrice { get; set; } = MIN_PRICE;
		public double MaxFullPrice { get; set; } = MAX_PRICE;
	}
}
