using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Items;

public sealed record ItemId : ValueObject
{
    private ItemId(Guid value)
    {
        Value = value;
    }

    private ItemId() { }
    public Guid Value { get; }

    public static ItemId CreateUnique()
    {
        return new ItemId(Guid.NewGuid());
    }

    public static ItemId Create(Guid value)
    {
        return new ItemId(value);
    }
}
