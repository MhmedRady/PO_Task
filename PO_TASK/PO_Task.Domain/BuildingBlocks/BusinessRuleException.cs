namespace PO_Task.Domain.BuildingBlocks;

public class BusinessRuleException(IEnumerable<Error> errors) : Exception
{
    public IEnumerable<Error> Errors { get; } = errors;
}

