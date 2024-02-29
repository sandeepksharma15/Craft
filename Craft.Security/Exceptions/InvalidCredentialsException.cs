using System.Net;
using Craft.Domain.Exceptions;

namespace Craft.Security.Exceptions;

public class InvalidCredentialsException : CustomException
{
    public InvalidCredentialsException()
        : base("Invalid Credentials: Please check your credentials") { }

    public InvalidCredentialsException(string message) : base(message) { }

    public InvalidCredentialsException(string message, Exception innerException)
        : base(message, innerException) { }

    public InvalidCredentialsException(
        string message,
        List<string> errors = null,
        HttpStatusCode statusCode = HttpStatusCode.NotAcceptable)
            : base(message, errors, statusCode) { }
}
