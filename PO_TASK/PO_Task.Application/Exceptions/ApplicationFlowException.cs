namespace PO_Task.Application.Exceptions;

public class ApplicationFlowException(IEnumerable<ApplicationError> errors) : Exception
{
    public IEnumerable<ApplicationError> Errors { get; } = errors;
}
