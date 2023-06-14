using api.deli.ru.ConfigModels;
using api.deli.ru.Managers;

namespace api.deli.ru
{
    public static class Properties
	{
		public static ApiConfigurations? ApiConfigurations { get; } = ServiceManager.GetService<ApiConfigurations>();
	}
}
