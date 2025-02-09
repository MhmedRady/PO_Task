using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Domain.Items;

public sealed class PurchaseOrderItem : Entity<ItemId>, IAggregateRoot
{

    private PurchaseOrderItem(
        ItemId id,
        PurchaseOrderId purchaseOrderId,
        string goodCode,
        Money price,
        decimal quantity,
        int serialNumber
        ) : base(id)
    {
        PurchaseOrderId = purchaseOrderId;
        GoodCode = goodCode;
        Price = price;
        Quantity = quantity;
        SerialNumber = serialNumber;
    }

    private PurchaseOrderItem()
    {
    }

    public string GoodCode { get; private set; }
    public int SerialNumber { get; private set; }

    public PurchaseOrderId PurchaseOrderId { get; private set; }
    public Money Price { get; private set; }
    public decimal Quantity { get; private set; }

    public static PurchaseOrderItem CreateInstance(
        PurchaseOrderId purchaseOrderId,
        string goodCode,
        //int serialNumber,
        decimal quantity,
        Money price
        )
    {
        var SerialNumber = new Random().Next(100000);
        return new PurchaseOrderItem(
            ItemId.CreateUnique(),
            purchaseOrderId,
            goodCode,
            price,
            quantity,
            SerialNumber
            );
    }
}
