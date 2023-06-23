using api.deli.ru.Constants;

using Microsoft.AspNetCore.Authorization;

namespace api.deli.ru.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class AuthAttribute : AuthorizeAttribute
	{
		public string Scopes { get; set; } = AppScopes.All;
	}
}
