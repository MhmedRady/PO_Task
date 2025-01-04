using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.PurchaseOrders;

namespace PO_Task.Domain.Orders.OrderStrategies;

public sealed class OrderedStatusTransitionStrategy : IStatusTransitionStrategy<PurchaseOrder, PurchaseOrderStatus>
{
    public PurchaseOrderStatus From => PurchaseOrderStatus.Ordered;

    public PurchaseOrderStatus GetNextStatus(PurchaseOrder entity)
    {
        if (entity.Status == PurchaseOrderStatus.Ordered)
        {
            return ValidateTransition(PurchaseOrderStatus.Created);
        }

        if (entity.Status == PurchaseOrderStatus.Created)
        {
            return ValidateTransition(PurchaseOrderStatus.Approved);
        }

        if (entity.Status == PurchaseOrderStatus.ShipmentsInPreparation)
        {
            return ValidateTransition(PurchaseOrderStatus.ShipmentsIsShipped);
        }
        return ValidateTransition(PurchaseOrderStatus.Completed);
    }

    private PurchaseOrderStatus ValidateTransition(PurchaseOrderStatus newStatus)
    {
        return From.CanTransitionTo(newStatus)
            ? newStatus
            : throw new InvalidOperationException($"Invalid transition from {From.Name} to {newStatus.Name}.");
    }
}
