namespace PO_Task.Application.Exceptions;

public class ApplicationFlowException(List<ApplicationError> errors) : Exception
{
    public List<ApplicationError> Errors { get; } = errors;
}
