using System.Net;

namespace Craft.Domain.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, null, HttpStatusCode.NotFound) { }

    public NotFoundException() { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public NotFoundException(
        string message,
        List<string> errors = null,
        HttpStatusCode statusCode = HttpStatusCode.NotFound)
            : base(message, errors, statusCode) { }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.") { }
}
