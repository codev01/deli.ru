using System.Net;

namespace deli.api.Exceptions
{
	public class ResponseException : Exception
	{
        public HttpStatusCode StatusCode { get; }
		public ResponseException(Exception e, HttpStatusCode statusCode)
			=> StatusCode = statusCode;
		public ResponseException(string message, HttpStatusCode statusCode) : base(message) 
			=> StatusCode = statusCode;
	}
}
