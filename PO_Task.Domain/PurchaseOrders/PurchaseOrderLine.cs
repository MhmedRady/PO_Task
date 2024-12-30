using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Domain.Orders;

public sealed class PurchaseOrderLine : Entity<PurchaseOrderLineId>
{
    private PurchaseOrderLine(
        PurchaseOrderId purchaseOrderId,
        ItemId itemId,
        Money price,
        decimal quantity)
    {
        PurchaseOrderId = purchaseOrderId;
        ItemId = itemId;
        Price = price;
        Quantity = quantity;
    }

    private PurchaseOrderLine()
    {
    }

    public PurchaseOrderId PurchaseOrderId { get; private set; }
    public ItemId ItemId { get; private set; }
    public Money Price { get; private set; }
    public Money BasketPrice { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal BasketQuantity { get; private set; }

    public Item Item { get; set; }
    public string UnitName { get; }
    public Money Total { get; }
    public static PurchaseOrderLine Create(
        PurchaseOrderId purchaseOrderId,
        ItemId itemId,
        Money price,
        decimal quantity)
    {
        return new PurchaseOrderLine(
            purchaseOrderId,
            itemId,
            price,
            quantity);
    }
}
