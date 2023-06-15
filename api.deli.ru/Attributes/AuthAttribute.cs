using Microsoft.AspNetCore.Authorization;

namespace api.deli.ru.Attributes
{
	public class AuthAttribute
#if DEBUG
		: Attribute, IAuthorizeData
#elif RELEASE
		: AuthorizeAttribute
#endif

	{

#if DEBUG
		public string Policy { get; set; }
		public string Roles { get; set; }
		public string AuthenticationSchemes { get; set; }
#endif

	}
}
