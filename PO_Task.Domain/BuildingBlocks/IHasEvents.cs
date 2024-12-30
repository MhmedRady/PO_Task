namespace PO_Task.Domain.BuildingBlocks;

public interface IHasEvents
{
    public void ClearDomainEvents();
    public IReadOnlyList<IDomainEvent> GetDomainEvents();
}
