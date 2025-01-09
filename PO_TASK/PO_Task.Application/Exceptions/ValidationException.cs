namespace PO_Task.Application.Exceptions;

public sealed class ValidationException(IEnumerable<ValidationError> errors) : Exception
{

    public IEnumerable<ValidationError> Errors { get; } = errors;
}
