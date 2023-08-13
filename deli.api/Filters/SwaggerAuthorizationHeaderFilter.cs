using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace deli.api.Filters
{
	/// <summary>
	/// Определение фильтра операции для автоматического добавления заголовка Authorization
	/// </summary>
	public class SwaggerAuthorizationHeaderFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			//// Проверка наличия атрибута [Authorize] или [Authorize(Roles = "AllowedApplication")]
			//var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
			//	.Union(context.MethodInfo.GetCustomAttributes(true))
			//	.OfType<AuthorizeAttribute>()
			//	.Any();

			//if (hasAuthorizeAttribute)
			//{
			//	// Добавление заголовка Authorization с префиксом Bearer
			//	operation.Parameters ??= new List<OpenApiParameter>();
			//	operation.Parameters.Add(new OpenApiParameter
			//	{
			//		Name = "Authorization",
			//		In = ParameterLocation.Header,
			//		Description = "Access token",
			//		Required = true,
			//		Schema = new OpenApiSchema
			//		{
			//			Type = "string",
			//			Default = new OpenApiString("Bearer ")
			//		}
			//	});
			//}
		}
	}
}
