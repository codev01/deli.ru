using deli.api.ConfigModels;
using deli.api.Managers;

namespace deli.api
{
	public static class Properties
	{
		public static ApiConfigurations? ApiConfigurations { get; } = ApiManager.GetService<ApiConfigurations>();
	}
}
