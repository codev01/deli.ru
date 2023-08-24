using System.Net;

namespace deli.api.Exceptions
{
	public class ResponseException : Exception
	{
        public HttpStatusCode StatusCode { get; }
		public ResponseException(string message, HttpStatusCode statusCode) : base(message) 
			=> StatusCode = statusCode;
	}
}
