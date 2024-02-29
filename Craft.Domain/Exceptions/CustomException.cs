using System.Net;

namespace Craft.Domain.Exceptions;

public class CustomException : Exception
{
    public List<string> Errors { get; }
    public HttpStatusCode StatusCode { get; }

    public CustomException() : base() { }

    public CustomException(string message) : base(message) { }

    public CustomException(string message, Exception innerException) : base(message, innerException) { }

    public CustomException(
        string message,
        List<string> errors = default,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
    {
        Errors = errors;
        StatusCode = statusCode;
    }
}
