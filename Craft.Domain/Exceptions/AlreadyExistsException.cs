using System.Net;

namespace Craft.Domain.Exceptions;

public class AlreadyExistsException : CustomException
{
    public AlreadyExistsException()
        : base("This resource already exists") { }

    public AlreadyExistsException(string message) : base(message) { }

    public AlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }

    public AlreadyExistsException(
        string message,
        List<string> errors = null,
        HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
            : base(message, errors, statusCode) { }

    public AlreadyExistsException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exists") { }
}
