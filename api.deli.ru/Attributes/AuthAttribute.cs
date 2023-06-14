
using data.deli.ru;
using data.deli.ru.MongoDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

using NuGet.Common;

namespace api.deli.ru.Attributes
{
	public class AuthAttribute
#if DEBUG
		: Attribute
#elif AUTH_DEBUG
		: AuthorizeAttribute
#else
		: /*Authorize*/Attribute
#endif
	{
		
	}
}
