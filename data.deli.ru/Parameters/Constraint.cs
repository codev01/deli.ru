namespace data.deli.ru.Parameters
{
	public class Constraint
	{
		public const int SMALL_LIMIT = 100;
		public const int LARGE_LIMIT = 1000;

		/// <summary>
		/// Default value = <see cref="SMALL_LIMIT"/>
		/// </summary>
		public uint Limit { get; set; }
		public uint Offset { get; set; } = 0;


		public void ArgumentCheck(uint limit = SMALL_LIMIT)
		{
			if (limit < Limit)
				throw new ArgumentException($"_limit \"{limit}\" is greater than the set: {Limit}");
			Limit = limit;
		}
	}
}
