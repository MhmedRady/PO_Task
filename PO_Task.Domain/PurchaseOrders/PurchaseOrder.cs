using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace PO_Task.Domain.PurchaseOrders;

public class PurchaseOrder : Entity<PurchaseOrderId>, IAggregateRoot
{
    private readonly List<PurchaseOrderItem> _items = new();

    private PurchaseOrder() { }

    private PurchaseOrder(PurchaseOrderId orderId, UserId purchaserId)
    {
        Id = orderId;
        PurchaserId = purchaserId;
    }

    public UserId PurchaserId { get; private set; }
    public string PoNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Money TotalAmount { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public ReadOnlyCollection<PurchaseOrderItem> PurchaseOrderItems => _items.AsReadOnly();

    public PurchaseOrder CreateOrderInstance(
            PurchaseOrderId purchaseOrderId,
            UserId buyerId
        )
    {
        PurchaseOrder order = new PurchaseOrder(purchaseOrderId, buyerId);
        order.Status = PurchaseOrderStatus.Ordered;
        order.CreatedAt = DateTime.Now;
        return order;
    }

    public void AddOrderItem(string goodCode, Money price, int quantity)
    {
        if (_items.Any(item => item.GoodCode == goodCode))
            throw new InvalidOperationException($"Good with code {goodCode} already exists.");
        _items.Add(PurchaseOrderItem.CreateInstance(Id, goodCode, price, quantity, _items.Count + 1));
        RecalculateTotalAmount();
    }

    public void RemoveOrderItems(PurchaseOrderItem orderItem)
    {
        _items.Remove(orderItem);
    }

    public void UpdateOrderItems(PurchaseOrderItem orderItem)
    {
        _items.Remove(orderItem);
        _items.Add(orderItem);
    }


    public void ProcessNextStatus(IStatusTransitionStrategyFactory<PurchaseOrder, PurchaseOrderStatus> strategyFactory)
    {
        IStatusTransitionStrategy<PurchaseOrder, PurchaseOrderStatus> strategy = strategyFactory.GetStrategy(Status);
        Status = strategy.GetNextStatus(this);
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = _items.Aggregate(Money.Zero(), (total, item) => total.Add(item.Price));
    }
}
