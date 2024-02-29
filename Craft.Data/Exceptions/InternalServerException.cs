using System.Net;
using Craft.Domain.Exceptions;

namespace Craft.Data.Exceptions;

public class InternalServerException : CustomException
{
    public InternalServerException(string message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.InternalServerError) { }

    public InternalServerException() { }

    public InternalServerException(string message) : base(message) { }

    public InternalServerException(string message, Exception innerException)
        : base(message, innerException) { }

    public InternalServerException(
        string message,
        List<string> errors = null,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, errors, statusCode) { }
}
