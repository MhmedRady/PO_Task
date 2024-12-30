using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Orders;

public sealed record PurchaseOrderLineId : ValueObject
{
    private PurchaseOrderLineId(Guid value)
    {
        Value = value;
    }

    private PurchaseOrderLineId() { }
    public Guid Value { get; }

    public static PurchaseOrderLineId CreateUnique()
    {
        return new PurchaseOrderLineId(Guid.NewGuid());
    }

    public static PurchaseOrderLineId Create(Guid value)
    {
        return new PurchaseOrderLineId(value);
    }
}
