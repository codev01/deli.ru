using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace api.deli.ru.Schemas
{
	public enum ErrorType
	{
		/// <summary>
		/// Код ошибки не указан
		/// <para/>
		/// Error code not specified
		/// </summary>
		None,
		/// <summary>
		/// Непредвиденная ошибка
		/// <para/>
		/// Unexpected error
		/// </summary>
		Unexpected,
		NotFound,
		Unauthorized,
		MissingHeader,
		BadRequest
	}

	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class Error
	{
		public Error()
		{
			Code = ErrorType.None;
			Message = GetErrorTypeMessage(Code);
		}

		public Error(ErrorType errorType, Exception? exception = null)
		{
			Code = errorType;
			Message = GetErrorTypeMessage(errorType);
			_setExcept(exception);
		}

		public Error(ErrorType errorType, string msg, string actionName = null, Exception exception = null)
		{
			Code = errorType;
			Message = msg;
			ActionName = actionName;
			_setExcept(exception);
		}
		public ErrorType Code { get; set; }
		public string Message { get; set; }
		public string ActionName { get; set; }

		public Exception Exception { get; set; }

		public static string GetErrorTypeMessage(ErrorType errorType)
		{
			switch (errorType)
			{
				case ErrorType.None: return "Error code not specified";
				case ErrorType.Unexpected: return "Unexpected error";
				case ErrorType.Unauthorized: return "Unauthorized";
				default: return errorType.ToString();
			}
		}

		private void _setExcept(Exception exception)
		{
#if DEBUG
			Exception = exception;
#endif
		}
	}
}
