using Microsoft.AspNetCore.Authorization;

namespace api.deli.ru.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class AuthAttribute
#if DEBUG
		: Attribute, IAuthorizeData
#elif RELEASE
		: AuthorizeAttribute
#endif

	{

#if DEBUG
		public string Scopes { get; set; }
		public string Policy { get; set; }
		public string Roles { get; set; }
		public string AuthenticationSchemes { get; set; }
#endif

    }
}
