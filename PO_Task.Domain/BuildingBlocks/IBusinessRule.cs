namespace PO_Task.Domain.BuildingBlocks;

public interface IBusinessRule
{
    Error Error { get; }
    bool IsBroken();
}
