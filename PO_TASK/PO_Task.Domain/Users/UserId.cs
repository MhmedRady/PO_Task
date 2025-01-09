

using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Users;

public sealed record UserId : ValueObject
{
    private UserId(Guid value)
    {
        Value = value;
    }

    private UserId() { }
    public Guid Value { get; }

    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }
}
