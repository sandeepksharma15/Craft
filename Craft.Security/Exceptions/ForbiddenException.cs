using System.Net;
using Craft.Domain.Exceptions;

namespace Craft.Security.Exceptions;

public class ForbiddenException : CustomException
{
    public ForbiddenException(string message)
        : base(message, null, HttpStatusCode.Forbidden) { }

    public ForbiddenException() { }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException) { }

    public ForbiddenException(
        string message,
        List<string> errors = null,
        HttpStatusCode statusCode = HttpStatusCode.Forbidden)
            : base(message, errors, statusCode) { }
}
