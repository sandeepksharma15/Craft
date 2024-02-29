namespace Craft.Domain.Exceptions;

public class ModelValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ModelValidationException() : base("One or more validation failures have occurred.")
        => Errors = new Dictionary<string, string[]>();

    public ModelValidationException(string message) : base(message) { }

    public ModelValidationException(string message, Exception innerException)
        : base(message, innerException) { }
}
