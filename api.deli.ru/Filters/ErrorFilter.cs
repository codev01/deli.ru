using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

namespace api.deli.ru.Filters
{
	public class ErrorFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			if (context is not null)
			{
				context.Result = new ContentResult
				{
					Content = JsonConvert.SerializeObject(new Error
					{
						Code = ErrorType.Unexpected,
						Message = "Unexpected error",
						ActionName = context?.ActionDescriptor.DisplayName,
						Exception = context?.Exception
					},
					Formatting.Indented)
				};

				context!.ExceptionHandled = true;
			}
			else
				throw new Exception("context = null | ErrorFilterAttribute.OnException()");
		}
	}
}
