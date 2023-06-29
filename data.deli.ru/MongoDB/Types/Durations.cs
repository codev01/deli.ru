namespace data.deli.ru.MongoDB.Types
{
	/// <summary>
	/// Структура, представляющий правильный массив диапазонов часов
	/// </summary>
	public struct Durations
	{
		/// <summary>
		/// Количество диапазонов
		/// </summary>
		public const int LIMIT = 4;
		/// <summary>
		/// Количество часов в году
		/// </summary>
		public const double LIMIT_VALUE = 8765.81277;

		public static double[] DefaultHours = new double[LIMIT];

		/// <summary>
		/// Желаемые диапазоны часов для аренды
		/// </summary>
		public double[] Hours { get; set; }
        public Durations()
        {
            Hours = new double[LIMIT];
        }
        public Durations(params double[] hours)
		{
			if (hours.Length > LIMIT)
				throw new ArgumentException($"Parameter _limit exceeded \"{nameof(Durations)}\": \"{nameof(LIMIT)}\" = {LIMIT}");

			for (int i = 0; i < hours.Length; i++)
				if (hours[i] > LIMIT_VALUE)
					throw new ArgumentException($"Value is greater than allowed: \"{nameof(LIMIT_VALUE)}\"");

			Array.Sort(hours);
			Hours = hours;
		}

		public static Durations Create(double[] hours)
			=> new Durations(hours);
	}
	// 8765.81277
}
