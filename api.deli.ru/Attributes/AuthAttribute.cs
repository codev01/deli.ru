using api.deli.ru.Constants;

using Microsoft.AspNetCore.Authorization;

namespace api.deli.ru.Attributes
{
#if DEBUG_AUTHDISABLE
	public class AuthAttribute : Attribute, IAuthorizeData
	{
		public string Scopes { get; set; } = Constants.Scopes.All;
		public string Policy { get; set; }
		public string Roles { get; set; }
		public string AuthenticationSchemes { get; set; }
	}	
#else
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class AuthAttribute : AuthorizeAttribute
	{
		public string Scopes { get; set; } = Constants.Scopes.All;
	}
#endif

}
