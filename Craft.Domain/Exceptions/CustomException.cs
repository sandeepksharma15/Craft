using System.Net;

namespace Craft.Domain.Exceptions;

public abstract class CustomException : Exception
{
    public List<string> Errors { get; }
    public HttpStatusCode StatusCode { get; }

    protected CustomException() { }

    protected CustomException(string message) : base(message) { }

    protected CustomException(string message, Exception innerException) : base(message, innerException) { }

    protected CustomException(
        string message,
        List<string> errors = default,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
    {
        Errors = errors;
        StatusCode = statusCode;
    }
}
