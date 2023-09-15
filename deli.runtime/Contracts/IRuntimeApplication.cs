namespace deli.runtime.Contracts
{
	public interface IRuntimeApplication
	{
		/// <summary>
		/// Допускаемое количество запросов в секунду
		/// </summary>
		int RateLimit { get; }
	}
}
