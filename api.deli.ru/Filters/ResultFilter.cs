using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.deli.ru.Filters
{
	public class ResultFilter : IResultFilter
	{
		public void OnResultExecuting(ResultExecutingContext context)
		{
			switch ((object)context)
			{
				case ObjectResult or:
					break;
				case NotFound or NotFoundResult:
					break;
				default:
					break;
			}
			if (context.Result is ObjectResult objectResult)
			{
				//objectResult.StringId = new ApiResult { Common = objectResult.StringId };
			}
		}

		public void OnResultExecuted(ResultExecutedContext context)
		{
		}
	}

}
