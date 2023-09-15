using deli.runtime.Bases;
using deli.runtime.Contracts;

namespace deli.runtime.Containers
{
	public class ApplicationContainer_ : Container_Base, IRuntimeApplication
	{
		/// <summary>
		/// Допускаемое количество запросов в секунду
		/// </summary>
		public int RateLimit { get; }

		public ApplicationContainer_(string key, string token, int rateLimit) : base(key, token)
			=> RateLimit = rateLimit;

		public bool CheckRateLimit()
		{
			var updatedTime = DateTime.Now.AddMilliseconds(-(1000 / RateLimit));
			return LastCall < updatedTime;
		}
	}
}
