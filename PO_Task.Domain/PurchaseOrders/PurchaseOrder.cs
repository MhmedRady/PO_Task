using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.PurchaseOrders;
using PO_Task.Domain.Users;
using PO_Task.Domain.Users.Events;
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

    public static PurchaseOrder CreateOrderInstance(
            PurchaseOrderId purchaseOrderId,
            UserId buyerId,
            DateTime IssueDate,
            IEnumerable<PurchaseOrderItem> purchaseOrderItems
        )
    {
        PurchaseOrder order = new PurchaseOrder(purchaseOrderId, buyerId);
        
        order.PoNumber = CreatePoNumbre();
        order.Status = PurchaseOrderStatus.Created;
        order.CreatedAt = IssueDate;
        foreach (var poItem in purchaseOrderItems) 
        {
            order.AddOrderItem(poItem);
        }

        order.RaiseDomainEvent(new PurchaseOrderCreatedDomainEvent { Id = purchaseOrderId.Value, PONumber = order.PoNumber });

        return order;
    }

    public static string CreatePoNumbre()
    {
        var timeStaps = TimeSpan.FromMilliseconds(10);
        return $"PO-{timeStaps}";
    }

    public void AddOrderItem(PurchaseOrderItem purchaseOrderItem)
    {
        if (_items.Any(item => item.GoodCode == purchaseOrderItem.GoodCode))
            throw new InvalidOperationException($"Good with code {purchaseOrderItem.GoodCode} already exists.");
        _items.Add(purchaseOrderItem);
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
        RecalculateTotalAmount();
    }


    public void ProcessNextStatus(IStatusTransitionStrategyFactory<PurchaseOrder, PurchaseOrderStatus> strategyFactory)
    {
        IStatusTransitionStrategy<PurchaseOrder, PurchaseOrderStatus> strategy = strategyFactory.GetStrategy(Status);
        Status = strategy.GetNextStatus(this);
    }

    private void RecalculateTotalAmount()
    {
        if (HasMultipleCurrencyTypes())
            throw new BusinessRuleException([PurchaseOrderErrors.MultipleCurrencyTypes]);
        TotalAmount = _items.Aggregate(Money.Zero(_items.First().Price.Currency), (total, item) => total.Add(item.Price));
    }

    public bool HasMultipleCurrencyTypes()
    {
        // Extract distinct currency codes and check if there is more than one
        return _items
            .Select(item => item.Price.Currency).Distinct().Count() > 1;
    }
}
