using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Domain.Items;

public sealed class Item : Entity<ItemId>, IAggregateRoot, ISoftDelete
{

    private Item(
        ItemId id,
        string goodCode,
        Money price,
        decimal quantity,
        int serialNumber
        ) : base(id)
    {
        GoodCode = goodCode;
        Price = price;
        Quantity = quantity;
        SerialNumber = serialNumber;
    }

    private Item()
    {
    }

    public string GoodCode { get; private set; }
    public int SerialNumber { get; private set; }

    public PurchaseOrderId OrderId { get; private set; }
    public Money Price { get; private set; }
    public decimal Quantity { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }


    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
    }

    public static Item CreateInstance(
        
        string goodCode,
        Money price,
        decimal quantity,
        int serialNumber)
    {
        return new Item(
            ItemId.CreateUnique(),
            goodCode,
            price,
            quantity,
            serialNumber
            );
    }
}
