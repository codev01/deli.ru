namespace data.deli.ru.Parameters
{
	public class Constraint : Parameter<Constraint>
	{
		public const int SMALL_LIMIT = 100;
		public const int LARGE_LIMIT = 1000;
		private const int _DEFAULT_LIMIT = SMALL_LIMIT;

		/// <summary>
		/// Default value = <see cref="SMALL_LIMIT"/>
		/// </summary>
		public uint Limit { get; set; } = _DEFAULT_LIMIT;
		public uint Offset { get; set; } = 0;


		public void ArgumentCheck(uint limit = _DEFAULT_LIMIT)
		{
			if (limit < Limit)
				throw new ArgumentException($"_limit \"{limit}\" is greater than the set: {Limit}");
			Limit = limit;
		}
	}
}
