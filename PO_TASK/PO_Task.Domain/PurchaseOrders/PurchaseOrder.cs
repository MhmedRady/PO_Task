using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Common;
using PO_Task.Domain.Items;
using PO_Task.Domain.Users;
using PO_Task.Domain.Users.Events;
using System.Collections.ObjectModel;


namespace PO_Task.Domain.PurchaseOrders;


public class PurchaseOrder : Entity<PurchaseOrderId>, IAggregateRoot, ISoftDelete
{
    private readonly List<PurchaseOrderItem> _items = new();

    private PurchaseOrder() { }

    private PurchaseOrder(PurchaseOrderId orderId)
    {
        Id = orderId;
    }

    public string PoNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Money TotalAmount { get; private set; }
    // Is Deactivated? (on hold)
    public bool IsDeactivated { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public ReadOnlyCollection<PurchaseOrderItem> PurchaseOrderItems => _items.AsReadOnly();

    public static PurchaseOrder CreateOrderInstance(
            PurchaseOrderId purchaseOrderId,
            DateTime IssueDate,
            string PoNumber,
            IEnumerable<PurchaseOrderItem> purchaseOrderItems
        )
    {
        PurchaseOrder order = new PurchaseOrder(purchaseOrderId);

        order.PoNumber = PoNumber;
        order.Status = PurchaseOrderStatus.Created;
        order.CreatedAt = IssueDate;

        foreach (var poItem in purchaseOrderItems)
        {
            order.AddOrderItem(poItem);
        }

        order.RaiseDomainEvent(new PurchaseOrderCreatedDomainEvent { Id = purchaseOrderId.Value, PONumber = order.PoNumber });

        return order;
    }

    public static string CreatePoNumbre(DateTime createdDate)
    {
        var timeStaps = Int64.Parse(createdDate.ToString("yyyyMMddHHmmss")) + new Random().Next(1000);
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

    public void MarkAsDeleted()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
