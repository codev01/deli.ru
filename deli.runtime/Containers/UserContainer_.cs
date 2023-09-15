using System.Net;

using deli.runtime.Bases;

namespace deli.runtime.Containers
{
	public class UserContainer_ : Container_Base
	{
		public const int REMOTE_IP_UPDATE_MILLISECONDS = 500;
		public IPAddress IPAddress { get; private set; }
		public DateTime LastRemoteIpAddressUpdated { get; private set; } = DateTime.Now;

		public UserContainer_(string key, string token, IPAddress ipAddress) : base(key, token)
			=> IPAddress = ipAddress;

		public bool CheckIPAddress(IPAddress ipAddress)
		{
			try
			{
				if (ipAddress != IPAddress)
				{
					IPAddress = ipAddress;
					LastRemoteIpAddressUpdated = DateTime.Now;
				}
				var updatedTime = DateTime.Now.AddMilliseconds(-REMOTE_IP_UPDATE_MILLISECONDS);
				return LastRemoteIpAddressUpdated < updatedTime;
			}
			catch
			{
				return false;
			}
		}
	}
}
