using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.PurchaseOrders;

public sealed record PurchaseOrderId : ValueObject
{
    private PurchaseOrderId(Guid value)
    {
        Value = value;
    }

    private PurchaseOrderId() { }
    public Guid Value { get; }

    public static PurchaseOrderId CreateUnique()
    {
        return new PurchaseOrderId(Guid.NewGuid());
    }

    public static PurchaseOrderId Create(Guid value)
    {
        return new PurchaseOrderId(value);
    }
}
