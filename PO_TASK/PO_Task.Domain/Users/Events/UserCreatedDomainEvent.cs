

using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Users.Events;

public sealed record UserCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
}
