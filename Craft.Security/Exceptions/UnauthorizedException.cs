using System.Net;
using Craft.Domain.Exceptions;

namespace Craft.Security.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
        : base(message, null, HttpStatusCode.Unauthorized) { }

    public UnauthorizedException() { }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException) { }

    public UnauthorizedException(
        string message,
        List<string> errors = null,
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
            : base(message, errors, statusCode) { }
}
