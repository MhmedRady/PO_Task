

using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Users.Events;

public sealed record PurchaseOrderCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public string PONumber { get; init; }
}
