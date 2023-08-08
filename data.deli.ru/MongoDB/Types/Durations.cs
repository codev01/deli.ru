using System.Collections;

using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;

namespace data.deli.ru.MongoDB.Types
{
	/// <summary>
	/// Структура, представляющий правильный массив диапазонов часов
	/// </summary>
	public class Durations
	{
		/// <summary>
		/// Количество диапазонов
		/// </summary>
		public const int LIMIT = 4;
		/// <summary>
		/// Количество часов в году
		/// </summary>
		public const double LIMIT_VALUE = Duration.MAX_DURATION;

		public static double[] DefaultHours = new double[1];

		/// <summary>
		/// Желаемые диапазоны часов для аренды
		/// </summary>
		public double[] Hours { get; set; }
		public double MaxDuration { get; set; }
		public double MinDuration { get; set; }

		public Durations()
        {
            Hours = DefaultHours;
        }

        public Durations(params double[] hours)
		{
			if (hours.Length != 0)
			{
				if (hours.Length > LIMIT)
					throw new ArgumentException($"Parameter _limit exceeded \"{nameof(Durations)}\": \"{nameof(LIMIT)}\" = {LIMIT}");

				for (int i = 0; i < hours.Length; i++)
					if (hours[i] > LIMIT_VALUE)
						throw new ArgumentException($"Value is greater than allowed: \"{nameof(LIMIT_VALUE)}\"");

				Array.Sort(hours);
				Hours = hours;
				MinDuration = hours.First();
				MaxDuration = hours.Last();
			}
			else
				Hours = DefaultHours;
		}

		public static Durations Create(double[] hours)
			=> new Durations(hours);

		public static implicit operator Durations(double[] hours)
			=> new Durations(hours);

		public static implicit operator double[](Durations duration)
			=> duration.Hours;

		public static implicit operator List<double>(Durations duration)
			=> duration.Hours.ToList();
	}
}
